using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiatachokBullet : MonoBehaviour
{

    [HideInInspector] public float damage;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CharacterHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
