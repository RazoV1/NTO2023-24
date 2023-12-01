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
        hasFuse = inventory.tryToDel(fuse, 1);
        if (!hasFuse) return;
        isPowered = true;
        fuseSpriteRenderer.sprite = fuseActiveSprite;
        powerLedSpriteRenderer.color = Color.yellow;
    }
    
    public void FusePowerOff()
    {
        if (!hasFuse) return;
        isPowered = false;
        hasFuse = false;
        fuseSpriteRenderer.sprite = fuseInactiveSprite;
        powerLedSpriteRenderer.color = Color.black;
    }
}
