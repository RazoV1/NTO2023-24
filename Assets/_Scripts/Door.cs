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
    public int currentState;
    //0 - красная
    //1 - зеленая
    //2 - синяя
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        
    }

    public void CloseDoor()
    {
        
    }

    public void PowerUpDoor()
    {
        
    }
    
    public void PowerOffDoor()
    {
        
    }
}
