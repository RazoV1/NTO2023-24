using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bee")
        {
            other.gameObject.GetComponent<BeeHealth>().TakeDamage(damage);
            
        }
        if (other.gameObject.tag == "Pooh")
        {
            VinniePoohBehaviour b = other.gameObject.GetComponent<VinniePoohBehaviour>();
            b.HP -= damage;
            if (b.HP <= 0)
            {
                b.isDead = true;
                b.animator.SetBool("isDead",true);
                b.GetComponent<CapsuleCollider>().enabled = false;
                b.agent.Stop();
            }
        }
        Destroy(gameObject);
    }
}
