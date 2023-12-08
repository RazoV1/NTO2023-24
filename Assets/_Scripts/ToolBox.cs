using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBox : Box
{
    
    [Header("Tool Box")]

    [SerializeField] private Item item;
    [SerializeField] private int countItem;
    [SerializeField] private GameObject adviceText;
    [SerializeField] private bool hasObjectsToActive;
    [SerializeField] private bool hasObjectsToInactive;

    private bool used = false;

    override public void Open()
    {
        if (isClosed && canUse)
        {
            UnlockBox();
        }
        else if (canUse && !isClosed && !used)
        {
            DropItem();
            if (hasObjectsToActive)
            {
                GetComponent<ObjectsToActiveInactive>().ActiveObjects();
            }
        }
    }

    private void DropItem()
    {
        inventory.gameObject.GetComponent<PlayerController1>().StartCoroutine("LootAnim");
        inventory.AddItem(item, countItem);
        adviceText.SetActive(false);
        used = true;
        if(!isLore) return;
        Camera.main.GetComponent<TaskbarManager>().NextTask();
    }
    
    override protected void OnTriggerEnter(Collider other)
    {
        if (isClosed && other.CompareTag("Player"))
        {
            inventory = other.GetComponent<Inventory>();
            closedAdviceText.SetActive(true);
            canUse = true;
        }
        
        else if (other.CompareTag("Player") && !used)
        {
            if (isLore)
            {
                if (Camera.main.GetComponent<TaskbarManager>().currentTask >= loreTask)
                {
                    inventory = other.GetComponent<Inventory>();
                    adviceText.SetActive(true);
                    canUse = true;
                }
            }
            
        }
    }
    
    virtual protected void OnTriggerExit(Collider other)
    {
        
        if (isClosed && other.CompareTag("Player"))
        {
            closedAdviceText.SetActive(false);
            canUse = false;
            closed_UI.SetActive(false);
        }
        
        if (other.CompareTag("Player"))
        {
            adviceText.SetActive(false);
            canUse = false;
        }
    }

}
