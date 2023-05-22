using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatamariControl : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    public Transform playerTransform;

    [SerializeField]
    public float rollSpeed = 30.0f;

    [SerializeField]
    private float size = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(GameManager.Instance.moveVelocity.magnitude != 0 && GameManager.Instance.rotateVelocity == 0)
        {
            Move();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Pickup") && other.transform.localScale.magnitude <= size)
        {
            other.transform.parent = transform;
            size += other.transform.localScale.magnitude;
        }
    }

    void Move()
    {
        Vector3 moveVelocity = GameManager.Instance.moveVelocity;
        Vector3 movement = (moveVelocity.z * playerTransform.forward) + (moveVelocity.x * playerTransform.right);
        rb.AddForce(movement * Time.fixedDeltaTime * rollSpeed * size);
    }
}
