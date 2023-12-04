using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiatachokGunParent : MonoBehaviour
{
    [SerializeField] private PiatachokGun gun;

    private void Start()
    {
        gun = gun.GetComponent<PiatachokGun>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gun.ShotByTrigger();
        }
    }
}
