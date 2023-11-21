using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    
    

    [SerializeField] private GameObject inventory;

    public void OpenInventory()
    {
        inventory.SetActive(true);
    }
    
    public void CloseInventory()
    {
        inventory.SetActive(false);
    }

    public bool IsInventoryOpen()
    {
        return inventory.activeSelf;

    }
}
