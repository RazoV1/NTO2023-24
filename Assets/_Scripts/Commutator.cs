using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Commutator : MonoBehaviour
{

    [SerializeField] private GameObject UiDialog;
    [SerializeField] private GameObject AdviceText;
    private bool canUse;
    private bool used = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !used)
        {
            AdviceText.SetActive(true);
            canUse = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !used)
        {
            AdviceText.SetActive(false);
            canUse = false;
        }
    }

    private void Update()
    {
        if (canUse)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                used = true;
                canUse = false;
                UiDialog.SetActive(true);
                AdviceText.SetActive(false);
            }
            
        }
    }
}
