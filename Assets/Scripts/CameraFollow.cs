using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Vector3 offset;

    void Follow()
    {
        Vector3 newPos = target.position;
        newPos.z = offset.z;
        newPos.x = target.position.x + offset.x;
        newPos.y = target.position.y + offset.y; 
        
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        Follow();
    }
}
