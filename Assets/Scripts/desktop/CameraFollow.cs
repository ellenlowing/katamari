using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 playerOffset;
    public Transform targetLookAt;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = playerTransform.position + playerTransform.forward * playerOffset.z + new Vector3(0, playerOffset.y, 0);
        transform.LookAt(targetLookAt);
    }
}
