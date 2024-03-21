using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronetextFollow : MonoBehaviour
{
    public Transform drone;
    public Vector3 offset;
    public void Update()
    {
        transform.position = new Vector3(drone.position.x,drone.position.y, transform.position.z) + offset;
    }
}
