using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompressorManager : MonoBehaviour
{
    public static CompressorManager Instance { get; private set; }

    public int fixedCompressorsCount;

    public void FixCompressor()
    {
        fixedCompressorsCount++;
        if (fixedCompressorsCount >= 8)
        {
            Camera.main.GetComponent<TaskbarManager>().NextTask();
            Destroy(gameObject);
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
