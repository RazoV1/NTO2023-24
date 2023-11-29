using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorsManager : MonoBehaviour
{
    private List<Door> doors;
    [SerializeField] private List<DoorController> doorControllers;

    [SerializeField] private Item timer;
    [SerializeField] private Item fuse;
    [SerializeField] private GameObject AdviceText;
    [SerializeField] private GameObject UI_manager;
    private Inventory inventory;


    public bool hasFuse;
    public bool hasTimer;
    
    public bool isCycled;
    
    private float cycleTime;
    private float currentCycleTime;
    
    private void Start()
    {
        doors = new List<Door>();
        inventory = GetComponent<Inventory>();
        GameObject[] massDoors = GameObject.FindGameObjectsWithTag("Door");
        foreach (var massDoor in massDoors)
        {
            doors.Add(massDoor.GetComponent<Door>());
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AdviceText.SetActive(false);
            UI_manager.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UI_manager.SetActive(true);
        }
        
        if (isCycled && hasTimer && hasFuse)
        {
            currentCycleTime -= Time.deltaTime;
            if (currentCycleTime <= 0)
            {
                ChangeAllShieldsPower();
                currentCycleTime = cycleTime;
            }
        }
    }
    
    public void ChangeAllShieldsPower()
    {
        foreach (var controller in doorControllers)
        {
            controller.isBroken = !controller.isBroken;
        }
    }

    public void ChangeAllDoorColors(int color)
    {
        foreach (var door in doors)
        {
            if (color == door.currentState && door.isPoweredUp)
            {
                door.ChangeState();
            }
        }
    }

    public void ChangeCycleTime(float time)
    {
        if(!hasFuse && !hasTimer) return;
        if (time == 0) isCycled = false;
        currentCycleTime = time;
        cycleTime = time;
    }
    
    public void PowerUpFuse()
    {
        if (inventory.tryToDel(fuse, 1) && !hasFuse)
        {
            hasFuse = true;
        }
    }
    
    public void PowerOffFuse()
    {
        hasFuse = false;
        inventory.AddItem(fuse, 1);
    }
    
    public void PowerUpTimer()
    {
        if (inventory.tryToDel(timer, 1) && !hasTimer)
        {
            hasTimer = true;
        }
    }
    
    public void PowerOffTimer()
    {
        hasTimer = false;
        inventory.AddItem(timer, 1);
    }
    
}
