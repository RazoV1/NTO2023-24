using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform shotSpawnPosition;
    [SerializeField] private GameObject waterShotPrefab;

    public state currentState;

    public enum state
    {
        slime,
        water
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (currentState == state.water)
            {
                WaterShot(2);
            }
        }
    }

    private void WaterShot(float xScale)
    {
        GameObject currentShot = Instantiate(waterShotPrefab);
        currentShot.transform.position = shotSpawnPosition.position;
        currentShot.GetComponent<Rigidbody>().AddForce
            (Vector3.right * xScale * 10f, ForceMode.Impulse);
    }
    
}
