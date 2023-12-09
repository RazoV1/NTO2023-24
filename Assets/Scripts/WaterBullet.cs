using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bee")
        {
            other.gameObject.GetComponent<BeeHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
