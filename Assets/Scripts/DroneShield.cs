using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneShield : MonoBehaviour
{
    public DroneController droneController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "projectile")
        {
            //droneController.currentLayers--;
            Destroy(collision.gameObject);
        }
    }
}
