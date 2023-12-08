using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToNextTask : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Camera.main.GetComponent<TaskbarManager>().NextTask();
            Destroy(this);
        }
    }
}
