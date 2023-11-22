using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    private Rigidbody rb;
    public float Maxspeed;
    public float currentSpeed;
    public float accelerationForce;
    public float jumpForce;
    private Vector3 movement;
    private bool is_in_air = false;


    void Jump()
    {
        if (!is_in_air)
        {
            is_in_air = true;
            //movement.y = jumpForce;
            rb.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
           // movement = new Vector3(0, jumpForce, 0);
        }
    }
    private void RegisterInput()
    {
        if (!is_in_air)
        {
            movement = Vector3.Lerp(rb.velocity,Vector3.zero,Time.deltaTime * accelerationForce);
            if (Input.GetKey(KeyCode.D))
            {
                currentSpeed = Mathf.Lerp(rb.velocity.x, Maxspeed, accelerationForce * Time.deltaTime);
                movement.x = currentSpeed;
                
            }
            else if (Input.GetKey(KeyCode.A))
            {
                currentSpeed = Mathf.Lerp(rb.velocity.x, -Maxspeed, accelerationForce * Time.deltaTime);
                movement.x = currentSpeed;
               
            }
            if (Input.GetKey(KeyCode.W))
            {
                currentSpeed = Mathf.Lerp(rb.velocity.z, Maxspeed, accelerationForce * Time.deltaTime);
                movement.z = currentSpeed;
                
            }
            else if (Input.GetKey(KeyCode.S))
            {
                currentSpeed = Mathf.Lerp(rb.velocity.z, -Maxspeed, accelerationForce * Time.deltaTime);
                movement.z = currentSpeed;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            rb.velocity = movement;
        }
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (is_in_air)
        {
            is_in_air = false;
            movement.y = 0;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        RegisterInput();
    }
}
