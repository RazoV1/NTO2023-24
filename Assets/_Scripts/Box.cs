using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Box")] [SerializeField] 
    protected bool isClosed;
    [SerializeField] protected bool hasObjectsToActive;
    [SerializeField] protected bool hasObjectsToInactive;
    [SerializeField] protected GameObject highlitningGameObject;
    public Inventory inventory;
    [Header("If isClosed")]
    [SerializeField] protected GameObject closedAdviceText;
    [SerializeField] protected Item key;
    [SerializeField] protected GameObject closed_UI;
    [SerializeField] protected GameObject closedPanelSprite;
    [SerializeField] protected GameObject openedPanelSprite;
    
    [Header("If isLore")]
    [SerializeField] protected bool isLore;
    [SerializeField] protected int loreTask;


    protected bool canUse;


    virtual protected void Highlitning()
    {
        if (highlitningGameObject == null) return;

        if (!isClosed)
        {
            if (isLore)
            {
                if (Camera.main.GetComponent<TaskbarManager>().currentTask == loreTask)
                {
                    highlitningGameObject.SetActive(true);
                }
                else
                {
                    highlitningGameObject.SetActive(false);
                }
            }

            else
            {
                highlitningGameObject.SetActive(true);
            }
        }
        else
        {
            highlitningGameObject.SetActive(false);
        }
    }

    virtual protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Open();
        }

        if (highlitningGameObject == null) return;
        Highlitning();
    }
    

    virtual protected void OnTriggerEnter(Collider other)
    {
        if (isClosed && other.CompareTag("Player"))
        {
            inventory = other.GetComponent<Inventory>();
            closedAdviceText.SetActive(true);
            canUse = true;
        }
    }

    virtual protected void OnTriggerExit(Collider other)
    {
        
        if (isClosed && other.CompareTag("Player"))
        {
            closedAdviceText.SetActive(false);
            canUse = false;
        }
        
        if (other.CompareTag("Player"))
        {
            closedAdviceText.SetActive(false);
            canUse = false;
            closed_UI.SetActive(false);
        }
    }
    
    virtual public void Open()
    {
        if (isClosed && canUse)
        {
            UnlockBox();
        }
    }
    
    virtual public void UnlockBox()
    {
        if (inventory.tryToDel(key, 1))
        {
            isClosed = false;
            canUse = false;
            closedAdviceText.SetActive(false);
            openedPanelSprite.SetActive(true);
            closedPanelSprite.SetActive(false);
            if(isLore) Camera.main.GetComponent<TaskbarManager>().NextTask();
            if(hasObjectsToActive) GetComponent<ObjectsToActiveInactive>().ActiveObjects();
            
        }
    }
}
