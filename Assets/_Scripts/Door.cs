using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Door : MonoBehaviour
{
    public bool isOpen;
    public bool isPoweredUp;
    public bool hasFuse;
    public int currentState;

    [SerializeField] private Item fuse;
    
    //0 - красная
    //1 - зеленая
    //2 - синяя
    private Animator animator;

    [SerializeField] private Inventory _inventory;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _inventory = _inventory.GetComponent<Inventory>();
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
            _inventory.DeleteItem(fuse, 1);
        }
    }
    
    public void PowerOffDoor()
    {
        isPoweredUp = false;
        if(hasFuse) _inventory.AddItem(fuse, 1);
        hasFuse = false;
    }
}
