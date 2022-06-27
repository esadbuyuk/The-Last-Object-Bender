using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public GameObject player;
    public Vector3 cameraOffset;

    
    void LateUpdate()
    {
        transform.position = player.transform.position + cameraOffset;
    }
}
