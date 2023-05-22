using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceControl : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    public Transform katamariTransform;

    private float currAngle = 180;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveWithKatamari();
        RotateAroundKatamari();
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Pickup"))
        {
            // TODO add force to prince in opposite direction
        }
    }

    void MoveWithKatamari()
    {        
        float currAngleRadian = currAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(currAngleRadian), 0, Mathf.Cos(currAngleRadian));
        offset.Scale(new Vector3(0.8f, 0.8f, 0.8f));
        Vector3 newPos = new Vector3(
            katamariTransform.position.x + offset.x,
            transform.position.y,
            katamariTransform.position.z + offset.z
        );
        rb.MovePosition(newPos);
    }

    void RotateAroundKatamari()
    {
        float rotateVelocity = GameManager.Instance.rotateVelocity;
        currAngle += rotateVelocity * Time.fixedDeltaTime;
        currAngle = currAngle % 360;
        Quaternion deltaRotation = Quaternion.Euler(0, currAngle + 180, 0);
        rb.MoveRotation(deltaRotation);
    }
}
