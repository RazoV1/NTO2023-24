using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenCompressor : MonoBehaviour
{
    [SerializeField]
    private GameObject adviceText;

    public bool canFix = false;

    private void Update()
    {
        if (canFix)
        {
            adviceText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                CompressorManager.Instance.FixCompressor();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            adviceText.SetActive(true);
            canFix = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            adviceText.SetActive(false);
            canFix = false;
        }
    }
}
