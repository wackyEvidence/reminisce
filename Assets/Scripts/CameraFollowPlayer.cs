using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 tempPosition; 
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Hero").transform; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        tempPosition  = transform.position;
        tempPosition.x = playerTransform.position.x;
        transform.position = tempPosition;
    }
}
