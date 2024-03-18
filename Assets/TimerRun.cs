using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerRun : MonoBehaviour
{
    private float Timer = 60f;

    private void Update()
    {
        Timer -= Time.deltaTime;
        GetComponent<TextMeshProUGUI>().text = $"00:{(int)Timer}";
    }
}
