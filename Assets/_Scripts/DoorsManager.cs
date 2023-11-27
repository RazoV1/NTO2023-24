using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorsManager : MonoBehaviour
{
    private List<Door> doors;

    private void Start()
    {
        doors = new List<Door>();
        GameObject[] massDoors = GameObject.FindGameObjectsWithTag("Door");
        foreach (var massDoor in massDoors)
        {
            doors.Add(massDoor.GetComponent<Door>());
        }
    }

    public void ChangeAllDoorStates(int color)
    {
        foreach (var door in doors)
        {
            if (color == door.currentState && door.isPoweredUp)
            {
                door.ChangeState();
            }
        }
    }
    
}
