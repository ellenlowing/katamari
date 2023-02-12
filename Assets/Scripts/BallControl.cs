using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{

    // Character components
    private Rigidbody rb;

    // Player (da prince) state
    public GameObject player;
    public CharacterControl playerControl; // TODO replace with better way to grab velocity

    // Ball stats
    public float turn;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 inFront = player.transform.position + player.transform.forward;
        transform.position = new Vector3(inFront.x, 0, inFront.z);

        Quaternion playerUpRotation = Quaternion.FromToRotation(Vector3.up, player.transform.up);
        Quaternion forwardRotation = Quaternion.AngleAxis(playerControl.currentVelocity.z * Time.fixedDeltaTime * turn, player.transform.right);
        Quaternion rightRotation = Quaternion.AngleAxis(-playerControl.currentVelocity.x * Time.fixedDeltaTime * turn, player.transform.forward);
        transform.rotation = forwardRotation * rightRotation * playerUpRotation * transform.rotation;

    }
}
