using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    // Inputs
    private Vector3 inputL;
    private Vector3 inputR;

    // Character components
    private Rigidbody rb;

    // Player (da prince) state
    public Transform playerTransform;


    public float turn;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 inFront = playerTransform.position + playerTransform.forward;
        transform.position = new Vector3(inFront.x, 0, inFront.z);

        // inputL = new Vector3(Input.GetAxisRaw("LHorizontal"), 0, Input.GetAxisRaw("LVertical"));
        // inputR = new Vector3(Input.GetAxisRaw("RHorizontal"), 0, Input.GetAxisRaw("RVertical"));

        // // Grab distances and dot product of the 2 joystick inputs
        // float dotLR = Vector3.Dot(inputL, inputR);
        // float distLR = Vector3.Distance(inputL, inputR);
        
        // Quaternion deltaRotation = new Quaternion(0, 0, 0, 0);

        // if(dotLR == 1.0f)
        // {
        //     // Moving ball
        //     if(inputL.x == 1.0f)
        //     {
        //         deltaRotation = Quaternion.Euler(playerTransform.forward * turn * Time.fixedDeltaTime);
        //     }
        //     else if (inputL.x == -1.0f)
        //     {
        //         deltaRotation = Quaternion.Euler(-playerTransform.forward * turn * Time.fixedDeltaTime);
        //     }
        //     else if (inputL.z == 1.0f)
        //     {
        //         deltaRotation = Quaternion.Euler(playerTransform.right * turn * Time.fixedDeltaTime);
        //     }
        //     else if (inputL.z == -1.0f)
        //     {
        //         deltaRotation = Quaternion.Euler(-playerTransform.right * turn * Time.fixedDeltaTime);
        //     }
        //     rb.MoveRotation(rb.rotation * deltaRotation);
        // }
        // rb.AddTorque(turn * rollAxis);
    }
}
