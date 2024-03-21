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
                b.agent.Stop();
                b.animator.SetBool("isDead",true);
                b.GetComponent<CapsuleCollider>().enabled = false;
                Destroy(b.poohCollider);
                
            }
        }
        if (other.gameObject.tag == "piatachok")
        {
            PiatachokBehaviour p = other.gameObject.GetComponent<PiatachokBehaviour>();
            p.hp -= damage;
            if (p.hp <= 0)
            {
                Destroy(p.gameObject);
            }
        }
        Destroy(gameObject);
    }
}
