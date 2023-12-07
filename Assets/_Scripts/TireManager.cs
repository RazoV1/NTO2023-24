using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireManager : PoweredBox
{
    [SerializeField] private List<Door> doorToChangeState;
    [SerializeField] private List<LightManager> lightToChangeState;


    [SerializeField] private bool hasTimer;
    [SerializeField] private bool isCycled;
    [SerializeField] private SpriteRenderer tireManagerSpriteRenderer;
    [SerializeField] private GameObject AdviceText;
    [SerializeField] private GameObject UI_manager;
    [SerializeField] private Item timer;
    [SerializeField] private SpriteRenderer cycledSprite;
    
    private float cycleTime;
    private float currentCycleTime;

    private int currentTireColorTimer;
    
    private void Start()
    {
        canUse = false;
        fuseSpriteRenderer = fuseSpriteRenderer.GetComponent<SpriteRenderer>();
        tireManagerSpriteRenderer = tireManagerSpriteRenderer.GetComponent<SpriteRenderer>();
        inventory = GetComponent<Inventory>();
    }
    
    override protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canUse = true;
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canUse = false;
            AdviceText.SetActive(false);
            UI_manager.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUse)
        {
            UI_manager.SetActive(true);
        }
        
        if (isCycled && hasTimer && hasFuse)
        {
            currentCycleTime -= Time.deltaTime;
            if (currentCycleTime <= 0)
            {
                if (!isPowered || !hasTimer || !hasFuse) return;
                ChangeAllTires();
                if(cycledSprite.color == Color.black) cycledSprite.color = Color.yellow;
                else if(cycledSprite.color == Color.yellow) cycledSprite.color = Color.black;
                currentCycleTime = cycleTime;
            }
        }
    }
    
    public void ChangeAllTires()
    {
        if (!isPowered || !hasFuse) return;
        for (int i = 0; i <= 4; i++)
        {
            foreach (var door in doorToChangeState)
            {
                if (door.doorController.securityState == SecurityState.manual) continue;
                if (door.currentState == i)
                {
                    if (door.isOpen) door.CloseDoor();
                    else if (!door.isOpen) door.OpenDoor();
                }
            }

            foreach (var light in lightToChangeState)
            {
                if (light.currentState == i)
                {
                    if (light.currentLightState == LightManager.lightState.normal ||
                        light.currentLightState == LightManager.lightState.emergency) light.ChangeCurrentLightState(0);
                    else if (light.currentLightState == LightManager.lightState.off) light.ChangeCurrentLightState(2);
                }
            }
        }

    }
    
    public void Timed(float time)
    {
        if (!isPowered || !hasTimer || !hasFuse) return;
        if (time > 0)
        {
            isCycled = true;
            cycleTime = time;
            currentCycleTime = time;
            cycledSprite.color = Color.yellow;
        }
        else
        {
            isCycled = false;
            cycledSprite.color = Color.black;
        }
    }
    
    public void ChangeColorTires(int color)
    {
        if (!isPowered || !hasFuse) return;
        foreach (var door in doorToChangeState)
        {
            if (door.currentState == color)
            {
                if(door.isOpen) door.CloseDoor();
                else if(!door.isOpen) door.OpenDoor();
            }
        }

        foreach (var light in lightToChangeState)
        {
            if (light.currentState == color)
            {
                if(light.currentLightState == LightManager.lightState.normal || light.currentLightState == LightManager.lightState.emergency) light.ChangeCurrentLightState(0);
                else if(light.currentLightState == LightManager.lightState.off) light.ChangeCurrentLightState(2);
            }
        }
        
    }
    
    public void PowerUpTimer()
    {
        if(hasTimer) return;
        if (inventory.tryToDel(timer, 1))
        {
            hasTimer = true;
        }
    }
    
    public void PowerOffTimer()
    {
        if(!hasTimer) return;
        hasTimer = false;
        inventory.AddItem(timer, 1);
    }
}
