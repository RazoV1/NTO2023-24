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
    
    [SerializeField] private SpriteRenderer doorControllerSpriteRenderer;
    [SerializeField] private SpriteRenderer fuseSpriteRenderer;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private bool canUse;


    public bool hasFuse;
    public bool hasTimer;
    
    public bool isCycled;
    
    private float cycleTime;
    private float currentCycleTime;
    
    private void Start()
    {
        canUse = false;
        fuseSpriteRenderer = fuseSpriteRenderer.GetComponent<SpriteRenderer>();
        doorControllerSpriteRenderer = doorControllerSpriteRenderer.GetComponent<SpriteRenderer>();
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
            canUse = true;
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canUse = false;
            AdviceText.SetActive(false);
            UI_manager.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUse)
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
        isCycled = true;
    }
    
    public void PowerUpFuse()
    {
        if (inventory.tryToDel(fuse, 1) && !hasFuse)
        {
            hasFuse = true;
            fuseSpriteRenderer.color = Color.white;
        }
    }
    
    public void PowerOffFuse()
    {
        hasFuse = false;
        fuseSpriteRenderer.color = Color.black;
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
