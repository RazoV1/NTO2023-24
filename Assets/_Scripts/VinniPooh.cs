using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class VinniPooh : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform playerTransform;

    private bool onTarget;
    private NavMeshAgent agent;

    private void Start()
    {
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

    private void Update()
    {
        transform.LookAt(transform.position + new Vector3(0, 0, 1));
        if (Vector3.Distance(transform.position, playerTransform.position) <= 0.5f)
        {
            //Attack
        }
    }
}
