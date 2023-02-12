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
    public Vector3 accelTime = new Vector3(0.5f, 0, 2f);
    public Vector3 decelTime = new Vector3(1f, 0, 3f);
    public Vector3 brakeTime = new Vector3(0.5f, 0, 0.5f);
    public Vector3 maxSpeed = new Vector3(2f, 0, 6f);
    public float quickTurnAnglePerSec = 90f;
    public float turnAnglePerSec = 45f;

    // Stats calculated at runtime
    private Vector3 accelRatePerSec;
    private Vector3 decelRatePerSec;
    private Vector3 brakeRatePerSec;

    // Current character state
    private Vector3 currentVelocity;
    private float currentTurn;
    private bool inReverse;
    private bool accelChange;

    // Katamari state
    public Transform katamari;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        accelRatePerSec = new Vector3(maxSpeed.x / accelTime.x, 0, maxSpeed.z / accelTime.z);
        decelRatePerSec = new Vector3(-maxSpeed.x / decelTime.x, 0, -maxSpeed.z / decelTime.z);
        brakeRatePerSec = new Vector3(-maxSpeed.x / brakeTime.x, 0, -maxSpeed.z / brakeTime.z);
        currentVelocity = Vector3.zero;
        currentTurn = 0.0f;
        inReverse = false;
        accelChange = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        inputL = new Vector3(Input.GetAxisRaw("LHorizontal"), 0, Input.GetAxisRaw("LVertical"));
        inputR = new Vector3(Input.GetAxisRaw("RHorizontal"), 0, Input.GetAxisRaw("RVertical"));

        // Grab distances and dot product of the 2 joystick inputs
        float dotLR = Vector3.Dot(inputL, inputR);
        float distLR = Vector3.Distance(inputL, inputR);
        
        if(dotLR == 1.0f)
        {
            // Moving 
            if(Mathf.Abs(inputL.z) == 1.0f)
            {
                HandleAccelAndBrake(inputL.z, ref currentVelocity.z, ref currentVelocity.x, accelRatePerSec.z, brakeRatePerSec.z, brakeRatePerSec.x, maxSpeed.z);
            } 
            else 
            {
                HandleAccelAndBrake(inputL.x, ref currentVelocity.x, ref currentVelocity.z, accelRatePerSec.x, brakeRatePerSec.x, brakeRatePerSec.z, maxSpeed.x);
            }
        }
        else if (dotLR == -1.0f)
        {
            // quickly rotating, only if opposing axes are in Y
            float rotateDirection = Vector3.Dot(inputL, Vector3.forward);
            RotateAroundBall(quickTurnAnglePerSec * rotateDirection);
        }
        else if (dotLR == 0.0f && distLR == 1.0f)
        {
            // rotate slowly
            float rotateDirectionL = Vector3.Dot(inputL, Vector3.forward);
            float rotateDirectionR = Vector3.Dot(inputR, Vector3.back);

            if(Mathf.Abs(rotateDirectionL) > 0)
            {
                RotateAroundBall(turnAnglePerSec * rotateDirectionL);
            } 
            else 
            {
                RotateAroundBall(turnAnglePerSec * rotateDirectionR);
            }
        }
        
        if(!accelChange)
        {
            currentVelocity.z = Brake(currentVelocity.z, decelRatePerSec.z);
            currentVelocity.x = Brake(currentVelocity.x, decelRatePerSec.x);
        }

        rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, currentTurn, 0));
        rb.velocity = currentVelocity.z * transform.forward + currentVelocity.x * transform.right;

        // reset for next frame
        currentTurn = 0f;
        inputL = Vector3.zero;
        inputR = Vector3.zero;
        inReverse = false;
        accelChange = false;
    }

    void HandleAccelAndBrake(float input, ref float mainVelocity, ref float oppVelocity, float accelRate, float mainBrakeRate, float oppBrakeRate, float maxSpeed)
    {   
        float reverseFactor = Mathf.Sign(input);
        inReverse = reverseFactor == -1.0f ? true : false;
        if(mainVelocity * reverseFactor >= 0)
        {
            mainVelocity = Accelerate(mainVelocity, accelRate, maxSpeed);
            oppVelocity = Brake(oppVelocity, oppBrakeRate);
        }
        else
        {
            mainVelocity = Brake(mainVelocity, mainBrakeRate);
        }
    }

    float Accelerate(float velocity, float rate, float maxSpeed)
    {
        float reverseFactor = inReverse ? -1 : 1;
        velocity += rate * Time.deltaTime * reverseFactor;
        velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);
        accelChange = true;
        return velocity;
    }

    float Brake(float velocity, float rate)
    {
        if(velocity == 0)
        {
            return 0;
        }
        float reverseFactor = Mathf.Sign(velocity);
        velocity = Mathf.Abs(velocity);
        velocity += rate * Time.deltaTime;
        velocity = Mathf.Max(velocity, 0) * reverseFactor;
        accelChange = true;
        return velocity;
    }

    void RotateAroundBall(float anglePerSec)
    {
        transform.RotateAround(katamari.position, Vector3.up, anglePerSec * Time.deltaTime);
    }

}
