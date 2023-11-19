using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    private Rigidbody2D rb;
    public float Maxspeed;
    public float currentSpeed;
    public float accelerationForce;
    public float jumpForce;
    private Vector2 movement;
    private bool is_in_air;


    void Jump()
    {
        if (!is_in_air)
        {
            is_in_air = true;
            //movement.y = jumpForce;
            rb.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
        }
    }
    private void RegisterInput()
    {
        if (!is_in_air)
        {
            if (Input.GetKey(KeyCode.D))
            {
                currentSpeed = Mathf.Lerp(rb.velocity.x, Maxspeed, accelerationForce * Time.deltaTime);
                movement.x = currentSpeed;
                rb.velocity = movement;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                currentSpeed = Mathf.Lerp(rb.velocity.x, -Maxspeed, accelerationForce * Time.deltaTime);
                movement.x = currentSpeed;
                rb.velocity = movement;
            }
            if (Input.GetKey(KeyCode.W))
            {
                Jump();
            }
        }
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (is_in_air)
        {
            is_in_air = false;
            movement.y = 0;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        RegisterInput();
    }
}
