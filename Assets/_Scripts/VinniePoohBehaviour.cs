using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class VinniePoohBehaviour : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackCD;
    private float currentAttackCD;
    [SerializeField] private float damage;
    [SerializeField] private Transform playerTransform;
    private Animator animator;

    public bool onTarget;
    public bool onAttack;
    private NavMeshAgent agent;

    private void Start()
    {
        currentAttackCD = attackCD;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.SetDestination(playerTransform.position);
            onTarget = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !onAttack)
        {
            agent.SetDestination(playerTransform.position);
            onTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.SetDestination(transform.position);
            onTarget = false;
            if (transform.position.x < playerTransform.position.x)
            {
                animator.SetTrigger("rightIdle");
            }
            else if (transform.position.x > playerTransform.position.x)
            {
                animator.SetTrigger("leftIdle");
            }
        } 
    }

    private void Update()
    {
        currentAttackCD -= Time.deltaTime;
        
        if (Vector3.Distance(transform.position, playerTransform.position) <= 0.4f && currentAttackCD <= 0)
        {
            playerTransform.GetComponent<CharacterHealth>().TakeDamage(damage);
            currentAttackCD = attackCD;
            onAttack = true;
            agent.destination = transform.position;
            Invoke("OnAttackToFalse", currentAttackCD);
            onTarget = false;
            if (transform.position.x < playerTransform.position.x)
            {
                animator.SetTrigger("rightAttack");
            }
            else if (transform.position.x > playerTransform.position.x)
            {
                animator.SetTrigger("leftAttack");
            }

        }
        
        else if (onTarget && !onAttack)
        {
            if (transform.position.x < playerTransform.position.x)
            {
                animator.SetTrigger("rightWalk");
            }
            else if (transform.position.x > playerTransform.position.x)
            {
                animator.SetTrigger("leftWalk");
            }
        }
        
        transform.LookAt(transform.position + new Vector3(0, 0, 1));
    }

    private void OnAttackToFalse()
    {
        onAttack = false;
        agent.destination = playerTransform.position;
    }
}