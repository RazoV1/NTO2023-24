using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRigidbody : MonoBehaviour
{
    
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x + 400, 0));
        transform.Rotate(new Vector3(0, 0, 720));

    }

    
}
