using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private InventoryWindow inventoryWindow;
    

    public List<Item> inventoryItems = new List<Item>();
     public List<int> inventoryItemsCount = new List<int>();
    private PlayerController1 player;

    public Item testItem;
     
     private void Start()
     {
         //inventoryItemsCount = new List<int>();
         //inventoryItems = new List<Item>();
         inventoryWindow = inventoryWindow.GetComponent<InventoryWindow>();
        player = GetComponent<PlayerController1>();
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
        
        PlayerPrefs.SetString("InventoryItem_"+ (inventoryItems.Count -1), item.Name);
        PlayerPrefs.SetInt("InventoryItem_"+ (inventoryItems.Count -1) + "_Count", inventoryItemsCount[inventoryItems.Count -1]);
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
        PlayerPrefs.SetString("InventoryItem_"+ (inventoryItems.Count -1), item.Name);
        PlayerPrefs.SetInt("InventoryItem_"+ (inventoryItems.Count -1) + "_Count", inventoryItemsCount[inventoryItems.Count -1]);
        inventoryWindow.Redraw();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !player.is_coding)
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
    
    public int countItem(Item item)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].Name == item.GetComponent<Item>().Name)
            {
                print("1 HONEy");
                if (inventoryItemsCount[i] >= 1)
                {
                    return inventoryItemsCount[i];
                }
            }
        }
        return 0;
    }
    
    //Проверяет предмет на наличие, и если он есть в нужном кол-ве удаляет
    public bool tryToDel(Item item, int count)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].Name == item.Name)
            {
                if (inventoryItemsCount[i] >= count)
                {
                    inventoryItemsCount[i] -= count;
                    PlayerPrefs.SetString("InventoryItem_"+ (inventoryItems.Count -1), item.Name);
                    PlayerPrefs.SetInt("InventoryItem_"+ (inventoryItems.Count -1) + "_Count", inventoryItemsCount[inventoryItems.Count -1]);
                    return true;
                }
            }
        }
        return false;
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i<inventoryItems.Count; i++)
        {
            PlayerPrefs.SetInt("InventoryItem_"+ i + "_Count", inventoryItemsCount[i]);
        }
    }
}
