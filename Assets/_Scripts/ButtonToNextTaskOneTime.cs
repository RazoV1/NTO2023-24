using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToNextTaskOneTime : MonoBehaviour
{
    private bool used = false;

    [SerializeField] private int loreTask; 
    [SerializeField] private bool hasItemsToActive;
    [SerializeField] private bool hasItemsToInactive; 
    
    public void NextTask()
    {
        if (Camera.main.GetComponent<TaskbarManager>().currentTask >= loreTask)
        {
            if (used) return;
            else
            {
                Camera.main.GetComponent<TaskbarManager>().NextTask();
                used = true;
                if (hasItemsToActive)
                {
                    GetComponent<ObjectsToActiveInactive>().ActiveObjects();
                    hasItemsToActive = false;
                }
                if (hasItemsToInactive)
                {
                    GetComponent<ObjectsToActiveInactive>().InactiveObjects();
                    hasItemsToInactive = false;
                }
            }
        }
    }
}
