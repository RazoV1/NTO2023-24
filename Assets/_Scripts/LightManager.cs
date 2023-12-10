using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : PoweredBox
{
    [Header("Light Manager")]
    [SerializeField] private GameObject AdviceText;
    [SerializeField] public SpriteRenderer colorSprite;
    public GameObject UI_light;
    [SerializeField] private SpriteRenderer timedSprite;
    private bool isCycled;
    private float cycleTime;
    private float currentCycleTime;
    public int currentState;
    public string roomName;

    [SerializeField] private List<RoomMusicCollider> rooms;

    public enum lightState
    {
        off,
        emergency,
        normal
    }

    public lightState currentLightState;

    public Item key_uni;
    
    public bool isBroken;

    private bool isActive;

    private void Start()
    {
        colorSprite = colorSprite.GetComponent<SpriteRenderer>();
        timedSprite = timedSprite.GetComponent<SpriteRenderer>();
    }
    
    
    
    override protected void OnTriggerEnter(Collider other)
    {
        if (isClosed && other.CompareTag("Player"))
        {
            if (isLore)
            {
                if (Camera.main.GetComponent<TaskbarManager>().currentTask < loreTask)
                {
                    return;
                }
            }
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
            canUse = true;
        }
        
        else if (other.CompareTag("Player"))
        {
            
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
            canUse = true;
        }
    }

    override protected void OnTriggerExit(Collider other)
    {
        
        if (isClosed && other.CompareTag("Player"))
        {
            AdviceText.SetActive(false);
        }
        
        if (other.CompareTag("Player"))
        {
            AdviceText.SetActive(false);
            canUse = false;
            UI_light.SetActive(false);
        }
    }

    override protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isClosed && canUse)
            {
                UnlockBox();
            }
            
            if (canUse && !isClosed)
            {
                UI_light.SetActive(true);
            }
        }
        if(isBroken) return;
        if (isCycled)
        {
            currentCycleTime -= Time.deltaTime;
            if (currentCycleTime <= 0 && isActive)
            { 
                currentCycleTime = cycleTime;
                foreach (var room in rooms)
                {
                    switch (currentLightState)
                    {
                        case lightState.emergency:
                            ChangeCurrentLightState(0);
                            currentLightState = lightState.normal;
                            currentState = 2;
                            
                            break;
                        
                        case lightState.normal:
                            ChangeCurrentLightState(0);
                            currentLightState = lightState.normal;
                            currentState = 0;

                            break;
                        
                        case lightState.off:
                            ChangeCurrentLightState(2);
                            currentLightState = lightState.normal;
                            currentState = 0;

                            break;
                    }
                }
                currentCycleTime = cycleTime;
            }
            else if(currentCycleTime <= 0 && !isActive)
            {
                currentCycleTime = cycleTime;
                foreach (var room in rooms)
                {
                    room.OffLights();
                }
                currentCycleTime = cycleTime;
            }
        }
        
    }
    
    public void Timed(float time)
    {
        if (time > 0)
        {
            isCycled = true;
            cycleTime = time;
            currentCycleTime = time;
            timedSprite.color = Color.yellow;
        }
        else
        {
            isCycled = false;
            timedSprite.color = Color.black;
        }
    }
    
    public void ChangeColor(int color)
    {
        if (inventory.tryToDel(key_uni, 1))
        {
            switch (color)
            {
                case 0:
                    colorSprite.color = Color.red;
                    currentState = 0;
                    return;

                case 1:
                    colorSprite.color = Color.green;
                    currentState = 1;
                    return;

                case 2:
                    colorSprite.color = Color.blue;
                    currentState = 2;
                    return;

                case 3:
                    colorSprite.color = Color.white;
                    currentState = 3;
                    return;

            }
        }
    }

    public void ChangeCurrentLightState(int state)
    {
        if(!hasFuse) return;
        if(!isPowered) return;
        if (isBroken) return;
        foreach (RoomMusicCollider room in rooms)
        {
            print(room.gameObject.name);
            switch (state)
            {
                
                case 0:
                    room.currentRoomState = RoomMusicCollider.roomState.off;
                    room.OffLights();
                    break;
                        
                case 1:
                    room.currentRoomState = RoomMusicCollider.roomState.emergency;
                    room.EmergencyLightsOn();
                    break;
                
                case 2:
                    room.currentRoomState = RoomMusicCollider.roomState.normal;
                    room.NormalLightsOn();
                    break;
            }
        }
    }
    
    
}
