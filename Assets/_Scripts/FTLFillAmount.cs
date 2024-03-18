using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FTLFillAmount : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(delegate { FillImage();});
    }

    public void FillImage()
    {
        GameObject.Find("AWRSA").GetComponent<Image>().fillAmount += 0.125f;
        print(1);
    }
}
