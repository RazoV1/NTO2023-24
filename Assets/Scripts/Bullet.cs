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
    public float speed;


    private void Start()
    {
        //transform.localRotation = Quaternion.Euler(0, 0, 180 + MathF.Atan((transform.position.y - target.transform.position.y) / Vector2.Distance(transform.position, target.transform.position)) * (180 / MathF.PI));
    }
    private void Update()
    {
        //transform.LookAt(target.gameObject.transform.position);
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.gameObject.transform.position, 
            Time.deltaTime * speed);
        //transform.localRotation = Quaternion.Euler(0, 0, 180 + MathF.Atan((transform.position.y - target.transform.position.y) / Vector2.Distance(transform.position, target.transform.position)) * (180 / MathF.PI));

        if (Vector2.Distance(transform.position, target.transform.position) < 1f)
        {
            enemy.TakeDamage(target, canGoThroughShields, damage);
            Destroy(gameObject);
        }
    }
}
