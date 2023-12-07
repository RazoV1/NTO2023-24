using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredBox : Box
{
    [Header("Powered Box")]
    [SerializeField] protected Item fuse;
    [SerializeField] public bool isPowered;
    [SerializeField] public bool hasFuse;
    [SerializeField] public SpriteRenderer powerLedSpriteRenderer;
    [SerializeField] public SpriteRenderer fuseSpriteRenderer;
    [SerializeField] public Sprite fuseActiveSprite;
    [SerializeField] public Sprite fuseInactiveSprite;
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
        if(isLoreFuse) Camera.main.GetComponent<TaskbarManager>().NextTask();
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
        fuseSpriteRenderer.color = Color.black;
        powerLedSpriteRenderer.color = Color.black;
        
    }
}
