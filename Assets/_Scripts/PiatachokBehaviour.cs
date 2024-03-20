using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PiatachokBehaviour : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform bearParent;
    [SerializeField] private float  repulsionForce;

    [HideInInspector] public NavMeshAgent agent;

    public float xScale;

    private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject gunParent;
    public bool onShot;
    public bool isMovingRight;
    public bool isActive;

    public int hp;
    public bool isDead = false;
    
    private void Start()
    {
        xScale = 1;
        rb = GetComponent<Rigidbody>();
        animator = animator.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = movementSpeed;
    }
    public void OnShot(Vector3 moveVector)
    {
        StartCoroutine(ShotCoroutine(moveVector));
    }

    private void Update()
    {
        if (!isDead)
        {
            if (Vector3.Distance(transform.position, bearParent.position) > 0.4f) agent.destination = bearParent.position;
            else
            {
                if (xScale > 0)
                {
                    animator.SetTrigger("rightIdle");
                }
                else
                {
                    animator.SetTrigger("leftIdle");
                }
            }
            animator.gameObject.transform.LookAt(animator.transform.position + new Vector3(0, 0, 1));
            gun.transform.LookAt(gun.transform.position + new Vector3(0, 0, 1));
            if (onShot) return;
            else
            {
                if (bearParent.position.x > transform.position.x)
                {
                    xScale = 1;
                    if (!isMovingRight)
                    {
                        isMovingRight = true;
                        gunParent.transform.localScale = new Vector3(1, 1, 1);
                        return;
                    }
                    animator.SetTrigger("rightWalking");

                }
                else
                {
                    xScale = -1;
                    if (isMovingRight)
                    {
                        isMovingRight = false;
                        gunParent.transform.localScale = new Vector3(-1, 1, 1);
                        return;
                    }
                    animator.SetTrigger("leftWalking");
                }
            }
        }
    }

    private IEnumerator ShotCoroutine(Vector3 moveVector)
    {
        if (xScale > 0)
        {
            animator.SetTrigger("rightShot");
        }
        else
        {
            animator.SetTrigger("leftShot");
        }
        agent.speed = 0;
        onShot = true;
        rb.AddForce(moveVector * repulsionForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        print(12);
        agent.speed = movementSpeed;
        onShot = false;
    }
}
