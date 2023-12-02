using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSin : MonoBehaviour
{

    private Light light;
    [SerializeField] private float BaseLightIntesity;

    [SerializeField] private float SpeedSin;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void FixedUpdate()
    {
        light.intensity = ((Mathf.Sin(Time.time * SpeedSin) + BaseLightIntesity) / 2f);
    }
}
