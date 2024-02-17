using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BasicPart target;
    public int damage;
    public bool canGoThroughShields;
    public EnemyBehaviour enemy;

    private void Update()
    {
        //transform.LookAt(target.gameObject.transform.position);
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.gameObject.transform.position, 
            Time.deltaTime * 2);

        if (Vector2.Distance(transform.position, target.transform.position) < 1f)
        {
            enemy.TakeDamage(target, canGoThroughShields, damage);
            Destroy(gameObject);
        }
    }
}
