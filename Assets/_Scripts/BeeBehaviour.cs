using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBehaviour : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private int bulletsCount;
    [SerializeField] private int currentBulletsCount;
    [SerializeField] private float attackLongCooldown;
    [SerializeField] private float attackShortCooldown;
    [SerializeField] private float patrolZoneX;
    [SerializeField] private float followSpeed;

    [SerializeField] private float shortDistanceAttackRange;
    [SerializeField] private float longDistanceAttackRange;

    [SerializeField] private Transform playerTransform;

    private Vector3 startPosition;
    private bool isMovingRight;
    private bool isFollowingPlayer;
    
    [SerializeField] private int state; // 0 - обычная; 1 - относится ко входному шлюзу

    
    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        throw new NotImplementedException();
    }

    private void Move()
    {
        if (isFollowingPlayer)
        {
            Vector3 movementVector =
                new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, movementVector, followSpeed);
            if (Vector3.Distance(transform.position, movementVector) <= shortDistanceAttackRange)
            {
                
            }
        }
        else
        {
            if (isMovingRight)
            {
            
            }
        }
    }
    
}
