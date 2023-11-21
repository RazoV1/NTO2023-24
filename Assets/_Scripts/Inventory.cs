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
         inventoryWindow = inventoryWindow.GetComponent<InventoryWindow>();
     }
     
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
}
