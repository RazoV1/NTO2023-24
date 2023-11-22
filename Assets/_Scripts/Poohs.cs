using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class Poohs : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(250, 100));
        GetComponent<Rigidbody2D>().AddTorque(30f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(100, 250), UnityEngine.Random.Range(100, 250)));
        //GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x, -10f, 10f), Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.y, -10f, 10f));
    }
}
