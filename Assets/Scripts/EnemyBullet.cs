using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBullet : MonoBehaviour
{
    public BasicPart target;
    public int damage;
    public bool canGoThroughShields;
    public SpaceshipMainframe playerSpaceship;
    public float speed;

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 180+ MathF.Atan2((transform.position.y - target.transform.position.y), (transform.position.x - target.transform.position.x)) * (180 / MathF.PI));
        transform.position = Vector2.MoveTowards(transform.position,target.transform.position, Time.deltaTime * speed);
        if (Vector2.Distance(transform.position, target.transform.position) < 1f)
        {
            playerSpaceship.TakeDamage(target, canGoThroughShields, damage);
            Destroy(gameObject);
        }
    }
}