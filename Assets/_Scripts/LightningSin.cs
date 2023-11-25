using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSin : MonoBehaviour
{

    private Light light;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void FixedUpdate()
    {
        light.intensity = ((Mathf.Sin(Time.time * 1.5f) + 1f) / 2f);
    }
}
