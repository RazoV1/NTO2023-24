using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : BasicWeapon
{
    public LineRenderer lineRenderer;
    public Transform start;
    void CheckIfAiming()
    {
        if (is_selecting)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0,start.position);
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = 0;
            lineRenderer.SetPosition(1, p);
        }
        if (isOnCooldown)
        {
            lineRenderer.enabled = false;
        }
        
    }
    void Update()
    {
        CheckIfAiming();
        Cycle();
    }
}
