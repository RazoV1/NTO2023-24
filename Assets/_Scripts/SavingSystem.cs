using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    [SerializeField] private DoorController[] _doorControllers;
    [SerializeField] private List<Door> _doors;
    [SerializeField] private Commutator[] _commutators;

    private void Start()
    {
        _doors = new List<Door>();
        
        foreach (var doorController in _doorControllers)
        {
            _doors.Add(doorController.door);
        }
    }


    public void SaveAll()
    {
        //DoorControllers
        foreach (var doorController in _doorControllers)
        {
            if(doorController.isPowered)PlayerPrefs.SetInt("DoorController"+doorController.index+"_isPowered", 1);
            else if(doorController.isPowered)PlayerPrefs.SetInt("DoorController"+doorController.index+"_isPowered", 0);
            
            if(doorController.isBroken)PlayerPrefs.SetInt("DoorController"+doorController.index+"_isBroken", 1);
            else if(doorController.isBroken)PlayerPrefs.SetInt("DoorController"+doorController.index+"_isBroken", 0);
        }
        
        //Doors
        foreach (var door in _doors)
        {
            if(door.isOpen)PlayerPrefs.SetInt("DoorController"+door.index+"_isOpen", 1);
            else if(door.isOpen)PlayerPrefs.SetInt("DoorController"+door.index+"_isOpen", 0);
            
            if(door.currentState)PlayerPrefs.SetInt("DoorController"+door.index+"_isBroken", door.currentState);
            else if(door.currentState)PlayerPrefs.SetInt("DoorController"+door.index+"_isBroken", door.currentState);
            
        }
        
    }
    
}
