using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxWithInstuments : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private GameObject AdviceText;
    private bool canUse;
    private bool used = false;
    public bool isLore;

    private Inventory inventory;

    private TaskbarManager taskbarManager;

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
                if (taskbarManager.currentTask >= 1)
                {
                    inventory = other.GetComponent<Inventory>();
                    AdviceText.SetActive(true);
                    canUse = true;
                }

            }
            else
            {
                inventory = other.GetComponent<Inventory>();
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
        if (canUse)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                used = true;
                canUse = false;
                inventory.AddItem(item, 1);
                AdviceText.SetActive(false);
                if (isLore)
                {
                    taskbarManager.NextTask();
                }
            }
            
        }
    }
}
