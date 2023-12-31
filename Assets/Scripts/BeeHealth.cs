using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHealth : Health
{
    [SerializeField] private GameObject dedBee;

    private void Update()
    {
        if (isDead)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameObject ded_Bee = Instantiate(dedBee, transform.position, Quaternion.identity);
        ded_Bee.GetComponent<BeeDied>().IsActualBee = true;
    }
}
