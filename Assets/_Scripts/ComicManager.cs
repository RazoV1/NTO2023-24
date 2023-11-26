using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicManager : MonoBehaviour
{
    [SerializeField] private GameObject[] images;
    [Range(1f, 15f)][SerializeField] private float timeToChange;

    private float currentTimeToChange;
    private int currentSlide;


    private void Awake()
    {
        currentTimeToChange = timeToChange;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) currentTimeToChange = 0;
        
        Time.timeScale = 0;
        print(Time.deltaTime);
        if (currentSlide < images.Length - 1)
        {
            currentTimeToChange -= Time.deltaTime;
            if (currentTimeToChange <= 0)
            {
                currentSlide++;
                images[currentSlide].SetActive(true);
                currentTimeToChange = timeToChange;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                CloseComic();
            }
        }
        
    }

    public void CloseComic()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
