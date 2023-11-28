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
    public bool isPoweredUp;
    public bool hasFuse;
    public bool isCycled;
    private float cycleTime;
    private float currentCycleTime;
    public int currentState;

    [SerializeField] private Item fuse;
    
    //0 - красная
    //1 - зеленая
    //2 - синяя
    private Animator animator;

    [SerializeField] private Inventory _inventory;

    [SerializeField] private SpriteRenderer fuseSprite;
    [SerializeField] private SpriteRenderer isPoweredUpSprite;
    [SerializeField] private SpriteRenderer colorSprite;
    [SerializeField] private SpriteRenderer timedSprite;

    private void Start()
    {

        fuseSprite = fuseSprite.GetComponent<SpriteRenderer>();
        isPoweredUpSprite = isPoweredUpSprite.GetComponent<SpriteRenderer>();
        colorSprite = colorSprite.GetComponent<SpriteRenderer>();
        timedSprite = timedSprite.GetComponent<SpriteRenderer>();
        
        animator = GetComponent<Animator>();
        _inventory = _inventory.GetComponent<Inventory>();
    }

    private void Update()
    {
        if (isCycled)
        {
            if (currentCycleTime <= 0 && isOpen)
            {
                isOpen = false;
                animator.SetTrigger("close");
            }
            else if(currentCycleTime <= 0 && !isOpen)
            {
                isOpen = true;
                animator.SetTrigger("open");
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
        if (!isPoweredUp) return;
        isOpen = true;
        animator.SetTrigger("open");
    }

    public void CloseDoor()
    {
        if (!isOpen) return;
        if (!isPoweredUp) return;
        isOpen = false;
        animator.SetTrigger("close");
    }

    public void PowerUpDoor()
    {
        if (_inventory.tryToDel(fuse, 1) && !hasFuse)
        {
            hasFuse = true;
            isPoweredUp = true;
            fuseSprite.color = Color.white;
            isPoweredUpSprite.color = Color.yellow;
        }
    }
    
    public void PowerOffDoor()
    {
        isPoweredUp = false;
        if(hasFuse) _inventory.AddItem(fuse, 1);
        hasFuse = false;
        fuseSprite.color = Color.black;
        isPoweredUpSprite.color = Color.black;
    }
    
    public void Timed(float time)
    {
        if (time > 0)
        {
            isCycled = true;
        }
        else
        {
            isCycled = false;
        }
    }
    
    
    
}
