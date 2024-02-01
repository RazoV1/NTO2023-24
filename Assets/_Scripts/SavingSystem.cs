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
    [SerializeField] private FanManager[] _fanManagers;
    [SerializeField] private LightManager[] _lightManagers;
    [SerializeField] private DoorsManager[] _doorsManagers;

    private void Start()
    {

        if (PlayerPrefs.GetFloat("Player_X") == 0)
        {
            PlayerPrefs.SetFloat("Player_X", -9.48f);
            PlayerPrefs.SetFloat("Player_Y", -10.327f);
            PlayerPrefs.SetFloat("Player_Z", -0.77f);
        }
        else
        {
            LoadAll();
        }
        
        if (_doors != null)
        {
            _doors = new List<Door>();

            foreach (var doorController in _doorControllers)
            {
                _doors.Add(doorController.door);
            }
        }

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
            PlayerPrefs.SetFloat("Room"+room.index+"_oxygenForm", room.oxygenForm);
        }
        
        //Inventory
        PlayerPrefs.SetInt("InventoryCount", _player.GetComponent<Inventory>().inventoryItems.Count);
        /*for (int i = 0; i < _player.GetComponent<Inventory>().inventoryItems.Count; i++)
        {
            PlayerPrefs.SetString("InventoryItem_"+ i, _player.GetComponent<Inventory>().inventoryItems[i].ToString());
            print(_player.GetComponent<Inventory>().inventoryItems[i].GetComponent<Item>().Name + "_" + _player.GetComponent<Inventory>().inventoryItemsCount[i]);
            PlayerPrefs.SetInt("InventoryItem_"+ i +"_Count", _player.GetComponent<Inventory>().inventoryItemsCount[i]);
        }*/
        
        //Managers
        //TireManager
        foreach (var fan in _fanManagers)
        {
            if(fan.hasTimer)PlayerPrefs.SetInt("FanManager"+fan.index+"_hasTimer", 1);
            else if(fan.hasTimer)PlayerPrefs.SetInt("FanManager"+fan.index+"_hasTimer", 0);
            
            if(fan.isPowered)PlayerPrefs.SetInt("FanManager"+fan.index+"_isPowered", 1);
            else if(fan.isPowered)PlayerPrefs.SetInt("FanManager"+fan.index+"_isPowered", 0);
            
            if(fan.isBroken)PlayerPrefs.SetInt("FanManager"+fan.index+"_isBroken", 1);
            else if(fan.isBroken)PlayerPrefs.SetInt("FanManager"+fan.index+"_isBroken", 0);
            
            if(fan.hasFuse)PlayerPrefs.SetInt("FanManager"+fan.index+"_hasFuse", 1);
            else if(fan.hasFuse)PlayerPrefs.SetInt("FanManager"+fan.index+"_hasFuse", 0);
            
            if(fan.isCycled)PlayerPrefs.SetInt("FanManager"+fan.index+"_isCycled", 1);
            else if(fan.isCycled)PlayerPrefs.SetInt("FanManager"+fan.index+"_isCycled", 0);
            
            if(fan.currentFanState == FanManager.fanState.neutral) PlayerPrefs.SetInt("FanManager"+fan.index+"_fanState", 0);
            else if(fan.currentFanState == FanManager.fanState.pull) PlayerPrefs.SetInt("FanManager"+fan.index+"_fanState", 1);
            else if(fan.currentFanState == FanManager.fanState.push) PlayerPrefs.SetInt("FanManager"+fan.index+"_fanState", 2);
        }
        
        //LightManager
        foreach (var light in _lightManagers)
        {
            if(light.hasTimer)PlayerPrefs.SetInt("FanManager"+light.index+"_hasTimer", 1);
            else if(light.hasTimer)PlayerPrefs.SetInt("FanManager"+light.index+"_hasTimer", 0);
            
            if(light.isPowered)PlayerPrefs.SetInt("FanManager"+light.index+"_isPowered", 1);
            else if(light.isPowered)PlayerPrefs.SetInt("FanManager"+light.index+"_isPowered", 0);
            
            if(light.isBroken)PlayerPrefs.SetInt("FanManager"+light.index+"_isBroken", 1);
            else if(light.isBroken)PlayerPrefs.SetInt("FanManager"+light.index+"_isBroken", 0);
            
            if(light.hasFuse)PlayerPrefs.SetInt("FanManager"+light.index+"_hasFuse", 1);
            else if(light.hasFuse)PlayerPrefs.SetInt("FanManager"+light.index+"_hasFuse", 0);
            
            if(light.isCycled)PlayerPrefs.SetInt("FanManager"+light.index+"_isCycled", 1);
            else if(light.isCycled)PlayerPrefs.SetInt("FanManager"+light.index+"_isCycled", 0);
            
            if(light.currentLightState == LightManager.lightState.off) PlayerPrefs.SetInt("FanManager"+light.index+"_fanState", 0);
            else if(light.currentLightState == LightManager.lightState.emergency) PlayerPrefs.SetInt("FanManager"+light.index+"_fanState", 1);
            else if(light.currentLightState == LightManager.lightState.normal) PlayerPrefs.SetInt("FanManager"+light.index+"_fanState", 2);
        }
        
        //DoorManager
        foreach (var doorManager in _doorsManagers)
        {
            if(doorManager.hasTimer)PlayerPrefs.SetInt("FanManager"+doorManager.index+"_hasTimer", 1);
            else if(doorManager.hasTimer)PlayerPrefs.SetInt("FanManager"+doorManager.index+"_hasTimer", 0);
            
            if(doorManager.isPowered)PlayerPrefs.SetInt("FanManager"+doorManager.index+"_isPowered", 1);
            else if(doorManager.isPowered)PlayerPrefs.SetInt("FanManager"+doorManager.index+"_isPowered", 0);

            if(doorManager.hasFuse)PlayerPrefs.SetInt("FanManager"+doorManager.index+"_hasFuse", 1);
            else if(doorManager.hasFuse)PlayerPrefs.SetInt("FanManager"+doorManager.index+"_hasFuse", 0);
            
            if(doorManager.isCycled)PlayerPrefs.SetInt("FanManager"+doorManager.index+"_isCycled", 1);
            else if(doorManager.isCycled)PlayerPrefs.SetInt("FanManager"+doorManager.index+"_isCycled", 0);
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
            room.oxygenForm = PlayerPrefs.GetFloat("Room"+room.index+"_oxygenForm");
            
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
                if (item.name == PlayerPrefs.GetString("InventoryItem_" + i))
                {
                    _player.GetComponent<Inventory>().AddItem(item, PlayerPrefs.GetInt("InventoryItem_"+ i +"_Count"));
                }
            }
        }
        
        //Managers
        //TireManager
        foreach (var fan in _fanManagers)
        {
            if(PlayerPrefs.GetInt("DoorController"+fan.index+"_isPowered") == 1) fan.isPowered = true;
            else if(PlayerPrefs.GetInt("DoorController"+fan.index+"_isPowered") == 0) fan.isPowered = false;

            if(PlayerPrefs.GetInt("DoorController"+fan.index+"_isBroken") == 1) fan.isBroken = true;
            else if(PlayerPrefs.GetInt("DoorController"+fan.index+"_isBroken") == 0) fan.isBroken = false;
            
            if(PlayerPrefs.GetInt("DoorController"+fan.index+"_hasFuse") == 1) fan.hasFuse = true;
            else if(PlayerPrefs.GetInt("DoorController"+fan.index+"_hasFuse") == 0) fan.hasFuse = false;

            if(PlayerPrefs.GetInt("DoorController"+fan.index+"_hasTimer") == 1) fan.hasTimer = true;
            else if(PlayerPrefs.GetInt("DoorController"+fan.index+"_hasTimer") == 0) fan.hasTimer = false;
            
            if(PlayerPrefs.GetInt("FanManager"+fan.index+"_fanState") == 0) fan.currentFanState = FanManager.fanState.neutral;
            else if(PlayerPrefs.GetInt("FanManager"+fan.index+"_fanState") == 1) fan.currentFanState = FanManager.fanState.pull;
            else if(PlayerPrefs.GetInt("FanManager"+fan.index+"_fanState") == 2) fan.currentFanState = FanManager.fanState.push;
        }
        
        //LightManager
        foreach (var light in _lightManagers)
        {
            if(PlayerPrefs.GetInt("DoorController"+light.index+"_isPowered") == 1) light.isPowered = true;
            else if(PlayerPrefs.GetInt("DoorController"+light.index+"_isPowered") == 0) light.isPowered = false;

            if(PlayerPrefs.GetInt("DoorController"+light.index+"_isBroken") == 1) light.isBroken = true;
            else if(PlayerPrefs.GetInt("DoorController"+light.index+"_isBroken") == 0) light.isBroken = false;
            
            if(PlayerPrefs.GetInt("DoorController"+light.index+"_hasFuse") == 1) light.hasFuse = true;
            else if(PlayerPrefs.GetInt("DoorController"+light.index+"_hasFuse") == 0) light.hasFuse = false;

            if(PlayerPrefs.GetInt("DoorController"+light.index+"_hasTimer") == 1) light.hasTimer = true;
            else if(PlayerPrefs.GetInt("DoorController"+light.index+"_hasTimer") == 0) light.hasTimer = false;
            
            if(PlayerPrefs.GetInt("FanManager"+light.index+"_fanState") == 0) light.currentLightState = LightManager.lightState.off;
            else if(PlayerPrefs.GetInt("FanManager"+light.index+"_fanState") == 1) light.currentLightState = LightManager.lightState.emergency;
            else if(PlayerPrefs.GetInt("FanManager"+light.index+"_fanState") == 2) light.currentLightState = LightManager.lightState.normal;
        }
        
        //LightManager
        foreach (var light in _lightManagers)
        {
            if(PlayerPrefs.GetInt("DoorController"+light.index+"_isPowered") == 1) light.isPowered = true;
            else if(PlayerPrefs.GetInt("DoorController"+light.index+"_isPowered") == 0) light.isPowered = false;

            if(PlayerPrefs.GetInt("DoorController"+light.index+"_isBroken") == 1) light.isBroken = true;
            else if(PlayerPrefs.GetInt("DoorController"+light.index+"_isBroken") == 0) light.isBroken = false;
            
            if(PlayerPrefs.GetInt("DoorController"+light.index+"_hasFuse") == 1) light.hasFuse = true;
            else if(PlayerPrefs.GetInt("DoorController"+light.index+"_hasFuse") == 0) light.hasFuse = false;

            if(PlayerPrefs.GetInt("DoorController"+light.index+"_hasTimer") == 1) light.hasTimer = true;
            else if(PlayerPrefs.GetInt("DoorController"+light.index+"_hasTimer") == 0) light.hasTimer = false;
            
            if(PlayerPrefs.GetInt("FanManager"+light.index+"_fanState") == 0) light.currentLightState = LightManager.lightState.off;
            else if(PlayerPrefs.GetInt("FanManager"+light.index+"_fanState") == 1) light.currentLightState = LightManager.lightState.emergency;
            else if(PlayerPrefs.GetInt("FanManager"+light.index+"_fanState") == 2) light.currentLightState = LightManager.lightState.normal;
        }
        
        //DoorManager
        foreach (var doorManager in _doorsManagers)
        {
            if(PlayerPrefs.GetInt("DoorController"+doorManager.index+"_isPowered") == 1) doorManager.isPowered = true;
            else if(PlayerPrefs.GetInt("DoorController"+doorManager.index+"_isPowered") == 0) doorManager.isPowered = false;

            if(PlayerPrefs.GetInt("DoorController"+doorManager.index+"_hasFuse") == 1) doorManager.hasFuse = true;
            else if(PlayerPrefs.GetInt("DoorController"+doorManager.index+"_hasFuse") == 0) doorManager.hasFuse = false;

            if(PlayerPrefs.GetInt("DoorController"+doorManager.index+"_hasTimer") == 1) doorManager.hasTimer = true;
            else if(PlayerPrefs.GetInt("DoorController"+doorManager.index+"_hasTimer") == 0) doorManager.hasTimer = false;
        }
    }
    
    private void OnApplicationQuit()
    {
        SaveAll();
    }
}
