using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiatachokBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
