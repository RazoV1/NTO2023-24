using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTimeerTrigger : MonoBehaviour
{
    public DronePersonalitu drone;
    public bool isUsed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isUsed)
        {
            isUsed = true;
            drone.StartTick();
        }
    }
}
