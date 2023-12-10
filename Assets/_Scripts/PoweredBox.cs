using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredBox : Box
{
    [Header("Powered Box")]
    [SerializeField] protected Item fuse;
    [SerializeField] protected Item timer;
    [SerializeField] public bool isPowered;
    [SerializeField] public bool hasFuse;
    [SerializeField] public SpriteRenderer powerLedSpriteRenderer;
    [SerializeField] public SpriteRenderer fuseSpriteRenderer;
    [SerializeField] public Sprite fuseActiveSprite;
    [SerializeField] public Sprite fuseInactiveSprite;
    [SerializeField] public bool hasTimer;
    public SecurityState securityState;

    public enum SecurityState
    {
        manual,
        tire,
        programmator
    }
    
    
    [SerializeField] protected bool isLoreFuse;



    public void ChangeSecurityLevel(int level)
    {
        switch (level)
        {
            case 0:
                securityState = SecurityState.manual;
                break;

            case 1:
                securityState = SecurityState.tire;
                break;
            
            case 2:
                securityState = SecurityState.programmator;
                break;
            
        }
    }
    

    public void PowerOn()
    {
        print(gameObject.name);
        isPowered = true;
        if(!hasFuse) return;
        powerLedSpriteRenderer.color = Color.yellow;
        fuseSpriteRenderer.sprite = fuseActiveSprite;
        fuseSpriteRenderer.color = Color.white;
    }
    
    public void PowerDown()
    {
        isPowered = false;
        powerLedSpriteRenderer.color = Color.black;
        if(!hasFuse) return;
        fuseSpriteRenderer.sprite = fuseInactiveSprite;
        fuseSpriteRenderer.color = Color.white;
    }
    
    public void TryToFusePowerUp()
    {
        if (hasFuse) return;
        hasFuse = inventory.tryToDel(fuse, 1);
        if (!hasFuse) return;
        if(isLoreFuse && loreTask <= Camera.main.GetComponent<TaskbarManager>().currentTask) Camera.main.GetComponent<TaskbarManager>().NextTask();
        if (isPowered)
        {
            fuseSpriteRenderer.sprite = fuseActiveSprite;
            fuseSpriteRenderer.color = Color.white;
            powerLedSpriteRenderer.color = Color.yellow;
        }
        else
        {
            fuseSpriteRenderer.sprite = fuseInactiveSprite;
            fuseSpriteRenderer.color = Color.white;
        }
    }
    
    public void TryToFusePowerDown()
    {
        if (!hasFuse) return;
        hasFuse = false;
        inventory.AddItem(fuse, 1);
        fuseSpriteRenderer.color = Color.black;
        powerLedSpriteRenderer.color = Color.black;
        
    }
    
    public void TryToTimerPowerUp()
    {
        if (hasTimer) return;
        hasTimer = inventory.tryToDel(timer, 1);
    }
    
    public void TryToTimerPowerDown()
    {
        if (!hasTimer) return;
        hasTimer = false;
        inventory.AddItem(timer, 1);

    }
}
