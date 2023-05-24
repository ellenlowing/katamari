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
    public float katamariVolume = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        katamariVolume = GetComponent<EstimateVolume>().volume;
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
        float otherVolume = -1.0f;
        GameObject otherGameObj = other.gameObject;

        // Checks if the collided object has Pickup tag
        if(otherGameObj.CompareTag("Pickup"))
        {
            if(otherGameObj.GetComponent<EstimateVolume>() != null) 
            {
                // Grabs volume directly from collided
                otherVolume = otherGameObj.GetComponent<EstimateVolume>().volume;
            } 
            else 
            {
                // Grabs parent of the collided object and check its volume
                otherGameObj = other.gameObject.transform.parent.gameObject;
                otherVolume = otherGameObj.GetComponent<EstimateVolume>().volume;
            }
            if(otherVolume <= katamariVolume && otherVolume > 0)
            {
                // If volume is nonzero and smaller than total katamari volume, roll it up
                otherGameObj.transform.parent = transform;
                katamariVolume += otherVolume;
            } 
        }
    }

    void Move()
    {
        Vector3 moveVelocity = GameManager.Instance.moveVelocity;
        Vector3 movement = (moveVelocity.z * playerTransform.forward) + (moveVelocity.x * playerTransform.right);
        rb.AddForce(movement * Time.fixedDeltaTime * rollSpeed * Mathf.Pow(katamariVolume, 1/3));
    }
}
