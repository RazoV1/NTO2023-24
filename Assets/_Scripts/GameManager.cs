using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] soznanie_s;
    private int currentSoznanieIndex;


    private void Start()
    {
        Invoke("ActiveSoznanie", 3f);
    }

    private void ActiveSoznanie()
    {
        soznanie_s[currentSoznanieIndex].SetActive(true);
        currentSoznanieIndex++;
    }
}
