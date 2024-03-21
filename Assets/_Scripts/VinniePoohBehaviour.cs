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
    [HideInInspector] public Animator animator;

    public bool onTarget;
    public bool onAttack;
    [HideInInspector] public NavMeshAgent agent;

    [SerializeField] private float RushRange;
    [SerializeField] private float RushChance;
    [SerializeField] private bool RushDmgCd;

    private Vector3 RushPosition;
    private bool isRushing = false;
    public int HP;

    public bool isDead = false;

    [SerializeField] private AudioSource walkSound;

    private void Start()
    {
        currentAttackCD = attackCD;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            agent.SetDestination(playerTransform.position);
            walkSound.Play();
            onTarget = true;
            isRushing = true;
            Rush();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !onAttack && !isRushing && !isDead)
        {
            agent.SetDestination(playerTransform.position);
            onTarget = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            walkSound.Stop();
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
    private void Rush()
    {
        isRushing = true;
        RushPosition = playerTransform.position;
        agent.SetDestination(RushPosition);
        agent.speed = movementSpeed * 2f;
    }
    IEnumerator CDRushDamage()
    {
        RushDmgCd = false;
        yield return new WaitForSeconds(0.1f);
        RushDmgCd = true;
    }
    void RushDamage()
    {
       
        if (Vector3.Distance(transform.position, playerTransform.position) <= 0.2f && RushDmgCd && isRushing && !isDead)
        {
            playerTransform.GetComponent<CharacterHealth>().TakeDamage(1);
            StartCoroutine(CDRushDamage());
        }
        if (Vector3.Distance(transform.position,RushPosition) <= 0.2f && !isDead)
        {
            StopRush();
        }
    }
    private void StopRush()
    {
        isRushing = false;
        agent.speed = movementSpeed;
    }
    private void Update()
    {
        if (!isDead)
        {
            currentAttackCD -= Time.deltaTime;
            RushDamage();
            if (!isRushing)
            {
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
            }
            if (onTarget && !onAttack)
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
        
    }
    private void OnAttackToFalse()
    {
        onAttack = false;
        if (!isRushing)
        {
            agent.destination = playerTransform.position;
        }
    }
}