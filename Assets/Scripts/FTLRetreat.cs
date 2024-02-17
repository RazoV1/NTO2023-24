using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FTLRetreat : MonoBehaviour
{
    public GameObject MapCanvas;
    public Image image;
    public Button button;
    public float retreatFillSpeed;

    private void Start()
    {
        MapCanvas = GameObject.Find("Map");
        MapCanvas.active = false;
        button.interactable = false;
    }

    private void Update()
    {
        image.fillAmount += Time.deltaTime * retreatFillSpeed;
        if (image.fillAmount >= 1)
        {
            button.interactable = true;
        }
    }

    public void Retreat()
    {
        MapCanvas.SetActive(true);
        
        SceneManager.UnloadScene("SampleScene");
    }
}
