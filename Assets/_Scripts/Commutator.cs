using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Commutator : MonoBehaviour
{

    [SerializeField] private GameObject UiDialog;
    [SerializeField] private GameObject AdviceText;
    
    private bool canUse;
    private bool used = false;

    [SerializeField] private bool isLore;
    [SerializeField] private int loreTask;
    [SerializeField] private GameObject highlitning;
    

    private TaskbarManager taskbarManager;

    [Header("optional")] 
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject emergencyLightning;
    [SerializeField] private GameObject normalLightning;
    
    private void Start()
    {
        taskbarManager = Camera.main.GetComponent<TaskbarManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !used)
        {
            if (isLore)
            {
                if (loreTask <= Camera.main.GetComponent<TaskbarManager>().currentTask)
                {
                    AdviceText.SetActive(true);
                    canUse = true;
                }
            }
            else
            {
                AdviceText.SetActive(true);
                canUse = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !used)
        {
            AdviceText.SetActive(false);
            canUse = false;
        }
    }

    private void Update()
    {

        if (loreTask == Camera.main.GetComponent<TaskbarManager>().currentTask)
        {
            highlitning.SetActive(true);
        }
        else
        {
            highlitning.SetActive(false);
        }
        
        if (canUse)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                used = true;
                canUse = false;
                UiDialog.SetActive(true);
                AdviceText.SetActive(false);
                taskbarManager.NextTask();
                if (door != null)
                {
                    emergencyLightning.SetActive(false);
                    normalLightning.SetActive(true);
                    door.GetComponent<Door>().BackdoorOpenDoor();
                }
            }
            
        }
    }
}
