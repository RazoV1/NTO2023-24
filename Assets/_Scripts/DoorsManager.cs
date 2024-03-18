using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorsManager : PoweredBox
{
    [SerializeField] private List<PoweredBox> doorControllers;

    [SerializeField] private Item timer;
    //[SerializeField] private Item fuse;
    [SerializeField] private GameObject AdviceText;
    public GameObject UI_manager;
    public string name;
    //private Inventory inventory;
    
    [SerializeField] private SpriteRenderer doorControllerSpriteRenderer;
    //[SerializeField] private SpriteRenderer fuseSpriteRenderer;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    //private bool canUse;

    public bool isCycled;
    
    private float cycleTime;
    private float currentCycleTime;

    public int index;
    
    private void Start()
    {
        canUse = false;
        fuseSpriteRenderer = fuseSpriteRenderer.GetComponent<SpriteRenderer>();
        doorControllerSpriteRenderer = doorControllerSpriteRenderer.GetComponent<SpriteRenderer>();
        
        inventory = GetComponent<Inventory>();
    }
    
    override protected void OnTriggerEnter(Collider other)
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
        if (!hasFuse) return;
        foreach (var doorController in doorControllers)
        {
            switch (doorController.GetComponent<PoweredBox>().isPowered)
            {
                case false:
                    doorController.GetComponent<PoweredBox>().PowerOn();
                    print("f");
                    continue;
                case true:
                    doorController.GetComponent<PoweredBox>().PowerDown();
                    print("t");
                    continue;
            }
        }
    }

    public void ChangeCycleTime(float time)
    {
        if(!hasFuse || !hasTimer) return;
        if (time == 0)
        {
            isCycled = false;
            return;
        }
        currentCycleTime = time;
        cycleTime = time;
        isCycled = true;
    }
    
    
    public void PowerUpTimer()
    {
        if(hasTimer) return;
        if (inventory.tryToDel(timer, 1))
        {
            hasTimer = true;
        }
    }
    
    public void PowerOffTimer()
    {
        if(!hasTimer) return;
        hasTimer = false;
        inventory.AddItem(timer, 1);
    }
    
}
