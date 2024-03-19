using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTaskByTrigger : MonoBehaviour
{
    [SerializeField] private bool isLore;
    [SerializeField] private int loreTask;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isLore)
            {
                if (loreTask == Camera.main.GetComponent<TaskbarManager>().currentTask)
                {
                    Camera.main.GetComponent<TaskbarManager>().NextTask();
                }
            }
            else
            {
                Camera.main.GetComponent<TaskbarManager>().NextTask();
            }
        }
    }
}
