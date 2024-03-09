using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingControll : MonoBehaviour
{
    public GameObject mask;
    public int damage;
    private bool canDamage = false;
    [SerializeField] private float activeTime;
    [SerializeField] private float passiveTime;

    public IEnumerator cycle()
    {
        yield return new WaitForSeconds(passiveTime);
        canDamage = true;
        yield return new WaitForSeconds(activeTime);
        canDamage = false;
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
