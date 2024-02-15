using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FTLMapPointLogic : MonoBehaviour
{
    public bool isActive;
    public bool isPlayerOnPoint;
    public FTLMapPointLogic[] connectedPoints;
    public float minConnectDistance;

    private void Start()
    {
        
    }

    public void PlayerMoveOnPoint()
    {
        if (isActive)
        {
            isPlayerOnPoint = true;
            isActive = false;
            foreach (FTLMapPointLogic point in connectedPoints)
            {
                point.isActive = true;
            }
        }
    }
}
