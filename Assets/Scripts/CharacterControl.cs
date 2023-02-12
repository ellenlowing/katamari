using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    // Inputs
    private Vector3 inputL;
    private Vector3 inputR;

    // Character components
    private Animator animator;
    private Rigidbody rb;

    // Character stats
    public float accelTime = 2.5f;
    public float decelTime = 6f;
    public float brakeTime = 1f;
    public float maxSpeed = 6f;
    public float quickTurnAnglePerSec = 90f;
    public float turnAnglePerSec = 45f;

    // Stats calculated at runtime
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float brakeRatePerSec;

    // Current character state
    private float forwardVelocity;
    private float currentTurn;
    private bool inReverse;
    private bool accelChange;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        accelRatePerSec = maxSpeed / accelTime;
        decelRatePerSec = -maxSpeed / decelTime;
        brakeRatePerSec = -maxSpeed / brakeTime;
        forwardVelocity = 0.0f;
        currentTurn = 0.0f;
        inReverse = false;
        accelChange = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        inputL = new Vector3(Input.GetAxisRaw("LHorizontal"), 0, Input.GetAxisRaw("LVertical"));
        inputR = new Vector3(Input.GetAxisRaw("RHorizontal"), 0, Input.GetAxisRaw("RVertical"));

        float dotLR = Vector3.Dot(inputL, inputR);
        float distLR = Vector3.Distance(inputL, inputR);
        
        if(dotLR == 1.0f)
        {
            if(inputL.z == 1.0f) 
            {
                // Add acceleration
                if(forwardVelocity >= 0)
                {
                    Accelerate(accelRatePerSec);
                } 
                else 
                {
                    Brake(brakeRatePerSec);
                }
            } 
            else if (inputL.z == -1.0f) 
            {
                // brake
                if(forwardVelocity > 0)
                {
                    Brake(brakeRatePerSec);
                } 
                else 
                {
                    inReverse = true;
                    Accelerate(accelRatePerSec);
                }
            } 
            else if (inputL.x == 1.0f) 
            {

            }
            else if (inputL.x == -1.0f)
            {

            }
        }
        else if (dotLR == -1.0f)
        {
            // quickly rotating, only if opposing axes are in Y
            float rotateDirection = Vector3.Dot(inputL, Vector3.back);
            Rotate(quickTurnAnglePerSec, rotateDirection);
            
        }
        else if (dotLR == 0.0f && distLR == 1.0f)
        {
            // rotate slowly
            float rotateDirectionL = Vector3.Dot(inputL, Vector3.back);
            float rotateDirectionR = Vector3.Dot(inputR, Vector3.forward);

            if(Mathf.Abs(rotateDirectionL) > 0)
            {
                Rotate(turnAnglePerSec, rotateDirectionL);
            } 
            else 
            {
                Rotate(turnAnglePerSec, rotateDirectionR);
            }
        }
        else
        {
            Brake(decelRatePerSec);
        }

        rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, currentTurn, 0));
        rb.velocity = forwardVelocity * transform.forward;

        // reset for next frame
        currentTurn = 0f;
        inputL = Vector3.zero;
        inputR = Vector3.zero;
        inReverse = false;
        accelChange = false;
    }

    void Accelerate(float rate)
    {
        float reverseFactor = inReverse ? -1 : 1;
        forwardVelocity += rate * Time.deltaTime * reverseFactor;
        forwardVelocity = Mathf.Clamp(forwardVelocity, -maxSpeed, maxSpeed);
        accelChange = true;
    }

    void Brake(float rate)
    {
        if(forwardVelocity == 0)
        {
            return;
        }
        float reverseFactor = Mathf.Sign(forwardVelocity);
        forwardVelocity = Mathf.Abs(forwardVelocity);
        forwardVelocity += rate * Time.deltaTime;
        forwardVelocity = Mathf.Max(forwardVelocity, 0) * reverseFactor;
        accelChange = true;
    }

    void Rotate(float rate, float dir)
    {
        currentTurn = rate * Time.deltaTime * dir;
    }

}
