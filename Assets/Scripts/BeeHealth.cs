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
        Instantiate(dedBee);
    }
}
