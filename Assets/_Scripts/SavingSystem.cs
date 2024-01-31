using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private DoorController[] _doorControllers;
    [SerializeField] private List<Door> _doors;
    [SerializeField] private Commutator[] _commutators;
    [SerializeField] private RoomMusicCollider[] _roomMusicColliders;
    [SerializeField] private Item[] _items;

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
        PlayerPrefs.SetInt("CurrentTask", Camera.main.GetComponent<TaskbarManager>().currentTask);
        
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
        
        //Commutators
        foreach (var commutator in _commutators)
        {
            if(commutator.gameObject.activeSelf)PlayerPrefs.SetInt("Commutator"+commutator.index+"_isActive", 1);
            else if(!commutator.gameObject.activeSelf)PlayerPrefs.SetInt("Commutator"+commutator.index+"_isActive", 0);
        }
        
        //Rooms
        foreach (var room in _roomMusicColliders)
        {
            PlayerPrefs.SetFloat("Room"+room.index+"_oxygen", room.oxygen);
            PlayerPrefs.SetInt("Room"+room.index+"_tire", room.tire);
            if(room.currentRoomStateInt == 0)PlayerPrefs.SetInt("Room"+room.index+"_currentRoomStateInt", 0);
            else if(room.currentRoomStateInt == 1)PlayerPrefs.SetInt("Room"+room.index+"_currentRoomStateInt", 1);
            else if(room.currentRoomStateInt == 2)PlayerPrefs.SetInt("Room"+room.index+"_currentRoomStateInt", 2);
        }
        
        //Inventory
        PlayerPrefs.SetInt("InventoryCount", _player.GetComponent<Inventory>().inventoryItems.Count);
        for (int i = 0; i < _player.GetComponent<Inventory>().inventoryItems.Count; i++)
        {
            PlayerPrefs.SetString("InventoryItem_"+ i.ToString(), _player.GetComponent<Inventory>().inventoryItems[i].GetComponent<Item>().Name);
            PlayerPrefs.SetInt("InventoryItem_"+ i.ToString()+"_Count", _player.GetComponent<Inventory>().inventoryItemsCount[i]);
        }
        
    }
    
    public void LoadAll()
    {
        //Character
        _player.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_X"), PlayerPrefs.GetFloat("Player_Y"), PlayerPrefs.GetFloat("Player_Z")); 
        Camera.main.GetComponent<TaskbarManager>().currentTask = PlayerPrefs.GetInt("CurrentTask");

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
        
        //Commutators
        foreach (var commutator in _commutators)
        {
            if(PlayerPrefs.GetInt("Commutator"+commutator.index+"_isActive") == 1) commutator.gameObject.SetActive(true);
            else if(PlayerPrefs.GetInt("Commutator"+commutator.index+"_isActive") == 0) commutator.gameObject.SetActive(false);
        }
        
        //Rooms
        foreach (var room in _roomMusicColliders)
        {
            room.oxygen = PlayerPrefs.GetFloat("Room"+room.index+"_oxygen");
            room.tire = PlayerPrefs.GetInt("Room"+room.index+"_tire");
            
            if(PlayerPrefs.GetInt("Room"+room.index+"_currentRoomStateInt") == 0) room.currentRoomState = RoomMusicCollider.roomState.off;
            else if(PlayerPrefs.GetInt("Room"+room.index+"_currentRoomStateInt") == 1) room.currentRoomState = RoomMusicCollider.roomState.emergency;
            else if (PlayerPrefs.GetInt("Room" + room.index + "_currentRoomStateInt") == 2) room.currentRoomState = RoomMusicCollider.roomState.normal;
        }
        
        //Inventory
        int invCount = PlayerPrefs.GetInt("InventoryCount");
        for (int i = 0; i < invCount; i++)
        {
            foreach (var item in _items)
            {
                if (item.Name == PlayerPrefs.GetString("InventoryItem_" + i))
                {
                    _player.GetComponent<Inventory>().inventoryItems.Add(item);
                    _player.GetComponent<Inventory>().inventoryItemsCount.Add(PlayerPrefs.GetInt("InventoryItem_"+ i.ToString()+"_Count"));
                }
            }
        }
    }
    
    private void OnApplicationQuit()
    {
        SaveAll();
    }
}
