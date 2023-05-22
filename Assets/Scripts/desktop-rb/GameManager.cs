using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set; }

    // Exposed to other components
    public Vector3 moveVelocity;
    public float rotateVelocity;

    public float quickTurnAnglePerSec = 90f;
    public float turnAnglePerSec = 45f;

    // Debug
    [SerializeField]
    private Vector3 inputL;
    [SerializeField]
    private Vector3 inputR;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        ManageInputs();
    }

    public void ManageInputs()
    {
        // Grab distances and dot product of the 2 joystick inputs
        inputL = new Vector3(Input.GetAxisRaw("LHorizontal"), 0, Input.GetAxisRaw("LVertical"));
        inputR = new Vector3(Input.GetAxisRaw("RHorizontal"), 0, Input.GetAxisRaw("RVertical"));
        float dotLR = Vector3.Dot(inputL, inputR);
        float distLR = Vector3.Distance(inputL, inputR);

        if(dotLR == 1.0f)
        {
            // Moving (this shd be fine cause joystick only allows for single direction at once)
            moveVelocity = new Vector3(inputL.x, 0, inputL.z);
        }
        else 
        {
            moveVelocity = Vector3.zero;
        }
        
        if (dotLR == -1.0f)
        {
            // quickly rotating, only if opposing axes are in Y
            float rotateDirection = Vector3.Dot(inputL, Vector3.forward);
            rotateVelocity = quickTurnAnglePerSec * rotateDirection;
        }
        else if (dotLR == 0.0f && distLR == 1.0f)
        {
            // rotate slowly
            float rotateDirectionL = Vector3.Dot(inputL, Vector3.forward);
            float rotateDirectionR = Vector3.Dot(inputR, Vector3.back);

            if(Mathf.Abs(rotateDirectionL) > 0)
            {
                rotateVelocity = turnAnglePerSec * rotateDirectionL;
            } 
            else 
            {
                rotateVelocity = turnAnglePerSec * rotateDirectionR;
            }
        }
        else
        {
            rotateVelocity = 0.0f;
        }
    }
}
