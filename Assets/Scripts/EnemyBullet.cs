using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBullet : MonoBehaviour
{
    public BasicPart target;
    public int damage;
    public bool canGoThroughShields;
    public SpaceshipMainframe playerSpaceship;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, MathF.Acos((transform.position.y - target.transform.position.y) / Vector2.Distance(transform.position, target.transform.position)) * (180 / MathF.PI));
        transform.position = Vector2.MoveTowards(transform.position,target.transform.position, Time.deltaTime * 2);
        if (Vector2.Distance(transform.position, target.transform.position) < 1f)
        {
            playerSpaceship.TakeDamage(target, canGoThroughShields, damage);
            Destroy(gameObject);
        }
    }
}