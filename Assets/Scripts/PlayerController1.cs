using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    private Rigidbody rb;
    public float Maxspeed;
    public float climbSpeed;
    public float currentSpeed;
    public float accelerationForce;
    public float jumpForce;
    private Vector3 movement;
    private bool is_in_air = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool is_climbing = false;
    private bool is_animating = false;
    private Vector2 lastDir;

    [SerializeField] private AudioSource[] stepsAudioSources;
    private float lastStepTime;

    void Jump()
    {
        
        if (!is_in_air)
        {
            animator.SetBool("IsFalling", true);
            is_in_air = true;
            //movement.y = jumpForce;
            rb.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
           // movement = new Vector3(0, jumpForce, 0);
        }
    }
    private void RegisterInput()
    {

        //print(Input.GetAxis("Horizontal"));
        //rb.velocity = new Vector3(Input.GetAxis("Horizontal") * Maxspeed, rb.velocity.y,
        //    Input.GetAxis("Vertical") * Maxspeed);

        lastStepTime -= Time.deltaTime;
        if (!is_in_air && animator.GetFloat("X") >= 0.3f || animator.GetFloat("X") <= -0.3f
                                           || animator.GetFloat("Y") >= 0.3f || animator.GetFloat("Y") <= -0.3f)
        {
            if (lastStepTime <= 0)
            {
                stepsAudioSources[Random.Range(0, 3)].Play();
                lastStepTime = 0.4f;
            }
        }
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
                if (!is_climbing)
                {
                    currentSpeed = Mathf.Lerp(rb.velocity.z, Maxspeed, accelerationForce * Time.deltaTime);
                    movement.z = currentSpeed;
                }
                else
                {
                    is_animating = true;
                    transform.position = new Vector3(transform.position.x,transform.position.y + climbSpeed * Time.deltaTime,transform.position.z);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (!is_climbing)
                {
                    currentSpeed = Mathf.Lerp(rb.velocity.z, -Maxspeed, accelerationForce * Time.deltaTime);
                    movement.z = currentSpeed;
                }
                else
                {
                    is_climbing = false;
                }
            }
            else
            {
                is_animating = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                is_climbing = false;
                Jump();
            }
            if (movement.normalized.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movement.normalized.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            animator.SetFloat("X",Input.GetAxis("Horizontal"));
            animator.SetFloat("Y", Input.GetAxis("Vertical"));
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                lastDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
            else
            {
                animator.SetFloat("X", lastDir.x * 0.001f);
                animator.SetFloat("Y", lastDir.y * 0.001f);
            }
            animator.SetBool("animating", is_animating);
            rb.velocity = movement;
        }
        
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "ladder")
        {
            rb.useGravity = false;
            animator.SetBool("IsClimbing",true);
            is_climbing = true;
            
        }
        if (is_in_air)
        {
            if (collision.gameObject.tag == "floor" || collision.gameObject.tag == "ladder")
            {
                rb.useGravity = false;
                is_in_air = false;
                animator.SetBool("IsFalling", false);
                movement.y = 0;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        is_in_air = true;
        animator.SetBool("IsFalling", true);
        rb.useGravity = true;
        if (collision.gameObject.tag == "ladder")
        {
            animator.SetBool("IsClimbing", false);
            is_climbing = false;
            rb.useGravity = true;
            if (!is_in_air)
            {
                Jump();
            }
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        RegisterInput();
    }
}
