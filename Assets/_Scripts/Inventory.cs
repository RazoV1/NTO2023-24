using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private InventoryWindow inventoryWindow;
    

    public List<Item> inventoryItems = new List<Item>();
     public List<int> inventoryItemsCount = new List<int>();
     
     private void Start()
     {
         inventoryItemsCount = new List<int>();
         inventoryItems = new List<Item>();
         inventoryWindow = inventoryWindow.GetComponent<InventoryWindow>();
     }
     
     // ReSharper disable Unity.PerformanceAnalysis
     public void AddItem(Item item, int count)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].Name == item.Name)
            {
                inventoryItemsCount[i] += count;
                return;
            }
        }
        inventoryItems.Add(item);
        inventoryItemsCount.Add(count);
        inventoryWindow.Redraw();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void DeleteItem(Item item, int count)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].Name == item.Name)
            {
                inventoryItemsCount[i] -= count;
                return;
            }
        }
        inventoryWindow.Redraw();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryWindow.Redraw();
            inventoryWindow.gameObject.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryWindow.gameObject.SetActive(false);
        }
    }

    public bool hasItem(Item item)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].Name == item.Name)
            {
                if (inventoryItemsCount[i] >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool tryToDel(Item item, int count)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].Name == item.Name)
            {
                if (inventoryItemsCount[i] >= count)
                {
                    inventoryItemsCount[i] -= count;
                    return true;
                }
            }
        }
        return false;
    }
}
