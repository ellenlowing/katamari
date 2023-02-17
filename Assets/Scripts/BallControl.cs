using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{

    // Character components
    private Rigidbody rb;
    private MeshCollider cl;

    // Player (da prince) state
    public GameObject player;
    public CharacterControl playerControl; // TODO replace with better way to grab velocity

    // Ball stats
    public float turn;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Roll();
    }

    void Roll()
    {
        Vector3 inFront = player.transform.position + player.transform.forward;
        transform.position = new Vector3(inFront.x, 0, inFront.z);

        Quaternion playerUpRotation = Quaternion.FromToRotation(Vector3.up, player.transform.up);
        Quaternion forwardRotation = Quaternion.AngleAxis(playerControl.currentVelocity.z * Time.fixedDeltaTime * turn, player.transform.right);
        Quaternion rightRotation = Quaternion.AngleAxis(-playerControl.currentVelocity.x * Time.fixedDeltaTime * turn, player.transform.forward);
        transform.rotation = forwardRotation * rightRotation * playerUpRotation * transform.rotation;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pickup")
        {
            GameObject clone = Instantiate(other.gameObject, other.transform.position, other.transform.rotation, transform);
            clone.transform.localScale = new Vector3(clone.transform.localScale.x / transform.localScale.x, clone.transform.localScale.y / transform.localScale.y, clone.transform.localScale.z / transform.localScale.z);
            Destroy(other.gameObject);
        }
    }
}
