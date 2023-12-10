using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanManager : PoweredBox
{
    [Header("Fan Manager")]
    [SerializeField] private GameObject AdviceText;
    [SerializeField] public SpriteRenderer colorSprite;
    [SerializeField] private GameObject UI_fan;
    [SerializeField] private SpriteRenderer timedSprite;
    private bool isCycled;
    private float cycleTime;
    private float currentCycleTime;
    public int currentState;

    [SerializeField] private List<RoomMusicCollider> rooms;

    public enum fanState
    {
        neutral, // нейтральный(без изменений)
        pull, // вытягивать кислород -
        push // подавать кислород +
    }

    public fanState currentFanState;

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
            UI_fan.SetActive(false);
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
                UI_fan.SetActive(true);
            }
        }
        if(isBroken) return;
        if (isCycled)
        {
            currentCycleTime -= Time.deltaTime;
            if (currentCycleTime <= 0)
            { 
                currentCycleTime = cycleTime;
                foreach (var room in rooms)
                {
                    switch (currentFanState)
                    {
                        case fanState.neutral:
                            room.oxygenForm = room.oxygenVelocity;
                            room.OxygenChange(room.oxygenForm);
                            currentFanState = fanState.push;
                            continue;
                        
                        
                        case fanState.pull:
                            room.oxygenForm = room.oxygenVelocity;
                            room.OxygenChange(room.oxygenForm);
                            currentFanState = fanState.push;
                            continue;
                        
                                                
                        case fanState.push:
                            room.oxygenForm = -room.oxygenVelocity;
                            room.OxygenChange(room.oxygenForm);
                            currentFanState = fanState.pull;
                            continue;
                    }
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

    public void ChangeCurrentFanState(int state)
    {
        if(!hasFuse) return;
        if(!isPowered) return;
        if (isBroken) return;
        foreach (var room in rooms)
        {
            switch (state)
            {
                case 0:
                    room.oxygenForm = 0;
                    room.OxygenChange(room.oxygenForm);
                    currentFanState = fanState.neutral;
                    continue;
                        
                        
                case 1:
                    room.oxygenForm = -room.oxygenVelocity;
                    room.OxygenChange(room.oxygenForm);
                    currentFanState = fanState.pull;
                    continue;
                        
                                                
                case 2:
                    room.oxygenForm = room.oxygenVelocity;
                    room.OxygenChange(room.oxygenForm);
                    currentFanState = fanState.push;
                    continue;
            }
        }
    }
}
