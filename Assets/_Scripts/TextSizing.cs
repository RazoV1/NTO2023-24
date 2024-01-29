using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSizing : MonoBehaviour
{
    void Start()
    {
        float xRes = Screen.currentResolution.width;
        print(xRes);
        GetComponent<TextMeshProUGUI>().fontSize = 36f * (xRes/1920f);
    }

}
