using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private DoorController[] _doorControllers;
    [SerializeField] private List<Door> _doors;
    [SerializeField] private Commutator[] _commutators;

    private void Start()
    {
        if (_doors != null)
        {
            _doors = new List<Door>();

            foreach (var doorController in _doorControllers)
            {
                _doors.Add(doorController.door);
            }
        }
        LoadAll();
    }


    public void SaveAll()
    {
        //Character
        PlayerPrefs.SetFloat("Player_X", _player.transform.position.x);
        PlayerPrefs.SetFloat("Player_Y", _player.transform.position.y);
        PlayerPrefs.SetFloat("Player_Z", _player.transform.position.z);
        
        //DoorControllers
        foreach (var doorController in _doorControllers)
        {
            if(doorController.isPowered)PlayerPrefs.SetInt("DoorController"+doorController.index+"_isPowered", 1);
            else if(doorController.isPowered)PlayerPrefs.SetInt("DoorController"+doorController.index+"_isPowered", 0);
            
            if(doorController.isBroken)PlayerPrefs.SetInt("DoorController"+doorController.index+"_isBroken", 1);
            else if(doorController.isBroken)PlayerPrefs.SetInt("DoorController"+doorController.index+"_isBroken", 0);
            
            if(doorController.hasFuse)PlayerPrefs.SetInt("DoorController"+doorController.index+"_hasFuse", 1);
            else if(doorController.hasFuse)PlayerPrefs.SetInt("DoorController"+doorController.index+"_hasFuse", 0);
            
            if(doorController.hasTimer)PlayerPrefs.SetInt("DoorController"+doorController.index+"_hasTimer", 1);
            else if(doorController.hasTimer)PlayerPrefs.SetInt("DoorController"+doorController.index+"_hasTimer", 0);
        }
        
        //Doors
        foreach (var door in _doors)
        {
            if(door.isOpen)PlayerPrefs.SetInt("Door"+door.index+"_isOpen", 1);
            else if(door.isOpen)PlayerPrefs.SetInt("Door"+door.index+"_isOpen", 0);
            
            PlayerPrefs.SetInt("DoorController"+door.index+"_currentState", door.currentState);
        }
        
    }
    
    public void LoadAll()
    {
        //Character
        _player.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_X"), PlayerPrefs.GetFloat("Player_Y"), PlayerPrefs.GetFloat("Player_Z"));

        //DoorControllers
        foreach (var doorController in _doorControllers)
        {
            if(PlayerPrefs.GetInt("DoorController"+doorController.index+"_isPowered") == 1) doorController.isPowered = true;
            else if(PlayerPrefs.GetInt("DoorController"+doorController.index+"_isPowered") == 0) doorController.isPowered = false;

            if(PlayerPrefs.GetInt("DoorController"+doorController.index+"_isBroken") == 1) doorController.isBroken = true;
            else if(PlayerPrefs.GetInt("DoorController"+doorController.index+"_isBroken") == 0) doorController.isBroken = false;
            
            if(PlayerPrefs.GetInt("DoorController"+doorController.index+"_hasFuse") == 1) doorController.hasFuse = true;
            else if(PlayerPrefs.GetInt("DoorController"+doorController.index+"_hasFuse") == 0) doorController.hasFuse = false;

            if(PlayerPrefs.GetInt("DoorController"+doorController.index+"_hasTimer") == 1) doorController.hasTimer = true;
            else if(PlayerPrefs.GetInt("DoorController"+doorController.index+"_hasTimer") == 0) doorController.hasTimer = false;

        }
        
        //Doors
        foreach (var door in _doors)
        {
            if(PlayerPrefs.GetInt("Door"+door.index+"_isOpen") == 1) door.isOpen = true;
            else if(PlayerPrefs.GetInt("Door"+door.index+"_isOpen") == 0) door.isOpen = false;

            door.currentState = PlayerPrefs.GetInt("DoorController"+door.index+"_currentState");
        }
        
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }
}
