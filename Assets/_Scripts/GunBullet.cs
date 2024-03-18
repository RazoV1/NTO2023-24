using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    private GameObject currentWater;
    private float deltaColor = 0;
    public GameObject Water;
    private void OnTriggerEnter(Collider other)
    {
        currentWater = Instantiate(Water);
        currentWater.transform.position = transform.position;
        gameObject.SetActive(false);
        
        Invoke("DestroySelf", 2f);
    }
    

    private void DestroySelf()
    {
        Destroy(gameObject);
        Destroy(currentWater);
    }
}
