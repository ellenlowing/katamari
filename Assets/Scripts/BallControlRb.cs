using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControlRb : MonoBehaviour
{
    [SerializeField]
    public CharacterControl playerControl; // TODO replace with better way to grab velocity

    [SerializeField]
    public float rollSpeed = 30.0f;

    [SerializeField]
    public Transform cameraTransform;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 movement = (playerControl.currentVelocity.z * cameraTransform.forward) + (playerControl.currentVelocity.x * cameraTransform.right);

        rb.AddForce(movement * Time.fixedDeltaTime * rollSpeed);
    }
}
