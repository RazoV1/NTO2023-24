using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingControll : MonoBehaviour
{
    public GameObject mask;
    public int damage;
    public bool canDamage = false;
    [SerializeField] private float activeTime;
    [SerializeField] private float passiveTime;

    private void Start()
    {
        StartCoroutine(cycle());
    }

    public IEnumerator cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(activeTime);
            transform.position += Vector3.forward * 10;
            canDamage = false;
            //print("Work over");
            //print("Start work");
            yield return new WaitForSeconds(passiveTime);
            canDamage = true;
            transform.position += Vector3.forward * -10;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (canDamage && other.name == "Player")
        {
            other.GetComponent<CharacterHealth>().TakeDamage(damage);
            canDamage = false;
        }
    }
}
