using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Door : MonoBehaviour
{
    public bool isOpen;
    public bool isCycled;
    private float cycleTime;
    private float currentCycleTime;
    public int currentState;

    private Animator animator;
    
    [SerializeField] private SpriteRenderer timedSprite;
    public DoorController doorController;
    

    private void Start()
    {
        doorController = doorController.GetComponent<DoorController>();
        timedSprite = timedSprite.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        if (isOpen)
        {
            animator.SetTrigger("open");
        }
    }

    private void Update()
    {
        if(doorController.isBroken) return;
        if (isCycled)
        {
            currentCycleTime -= Time.deltaTime;
            if (currentCycleTime <= 0 && isOpen)
            {
                currentCycleTime = cycleTime;
                isOpen = false;
                animator.SetTrigger("close");
                currentCycleTime = cycleTime;
            }
            else if(currentCycleTime <= 0 && !isOpen)
            {
                currentCycleTime = cycleTime;
                isOpen = true;
                animator.SetTrigger("open");
                currentCycleTime = cycleTime;
            }
        }
    }

    public void ChangeState()
    {
        if (isOpen)
        {
            isOpen = false;
            animator.SetTrigger("close");
        }
        else
        {
            isOpen = true;
            animator.SetTrigger("open");
        }
        
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        if (!doorController.isPowered) return;
        if(doorController.isBroken) return;
        isOpen = true;
        animator.SetTrigger("open");
    }
    
    public void BackdoorOpenDoor()
    {
        isOpen = true;
        animator.SetTrigger("open");
    }

    public void BackdoorCloseDoor()
    {
        isOpen = false;
        animator.SetTrigger("close");
    }
    
    public void CloseDoor()
    {
        if (!isOpen) return;
        if (!doorController.isPowered) return;
        if(doorController.isBroken) return;
        isOpen = false;
        animator.SetTrigger("close");
    }
    public void ChangeColor(int color)
    {
        if (doorController.inventory.tryToDel(doorController.key_uni, 1))
        {
            switch (color)
            {
                case 0:
                    doorController.colorSprite.color = Color.red;
                    currentState = 0;
                    return;

                case 1:
                    doorController.colorSprite.color = Color.green;
                    currentState = 1;
                    return;

                case 2:
                    doorController.colorSprite.color = Color.blue;
                    currentState = 2;
                    return;

                case 3:
                    doorController.colorSprite.color = Color.white;
                    currentState = 3;
                    return;

            }
        }
    }

    /*public void PowerUpDoor()
    {
        if (_inventory.tryToDel(fuse, 1) && !hasFuse)
        {
            hasFuse = true;
            fuseSprite.color = Color.white;
            fuseSprite.sprite = fuseNotPowered;
            if(!isPoweredUp) return;
            fuseSprite.sprite = fusePowered;
            isPoweredUpSprite.color = Color.yellow;
        }
    }
    
    public void PowerOffDoor()
    {
        if(hasFuse) _inventory.AddItem(fuse, 1);
        hasFuse = false;
        fuseSprite.color = Color.black;
        isPoweredUpSprite.color = Color.black;
    }*/
    
    public void Timed(float time)
    {
        if(!doorController.hasTimer) return;
        if (time > 0)
        {
            isCycled = true;
            //2 секунды занимает анимация двери
            cycleTime = time+ 2f;
            currentCycleTime = time;
            timedSprite.color = Color.yellow;
        }
        else
        {
            isCycled = false;
            timedSprite.color = Color.black;
        }
    }
    
    /*public void ChangeColor(int color)
    {
        switch (color)
        {
            case 0:
                if (inventory.tryToDel(key_red, 1))
                {
                    colorSprite.color = Color.red;
                    currentState = 0;
                }
                return;
            
            case 1:
                if (_inventory.tryToDel(key_green, 1))
                {
                    colorSprite.color = Color.green;
                    currentState = 1;
                }
                return;
            
            case 2:
                if (_inventory.tryToDel(key_blue, 1))
                {
                    colorSprite.color = Color.blue;
                    currentState = 2;
                }
                return;
            
            case 3:
                if (_inventory.tryToDel(key_blue, 1))
                {
                    colorSprite.color = Color.white;
                    currentState = 3;
                }
                return;
            
        }
    }*/


    
    
    
}
