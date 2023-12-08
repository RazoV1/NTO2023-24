using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToNextTaskOneTime : MonoBehaviour
{
    private bool used = false;

    [SerializeField] private int loreTask; 
    
    public void NextTask()
    {
        if (Camera.main.GetComponent<TaskbarManager>().currentTask >= loreTask)
        {
            if (used) return;
            else
            {
                Camera.main.GetComponent<TaskbarManager>().NextTask();
                used = true;
            }
        }
    }
}
