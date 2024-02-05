using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour
{
    public GameObject Slime;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Slimable")
        {
            Instantiate(Slime,collision.GetContact(0).point,Quaternion.Euler(0, 90f,0));
            
        }
        Destroy(gameObject);
    }
}
