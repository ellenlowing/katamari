using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceControl : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    public Transform katamariTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveWithKatamari();
    }

    void MoveWithKatamari()
    {        
        float zOffset = -1.0f;
        Vector3 newPos = new Vector3(
            katamariTransform.position.x,
            transform.position.y,
            katamariTransform.position.z + zOffset
        );
        rb.MovePosition(newPos);
    }

    void RotateAroundKatamari()
    {

    }
}
