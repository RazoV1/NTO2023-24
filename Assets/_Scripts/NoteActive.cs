using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class NoteActive : MonoBehaviour
{
    [SerializeField] private GameObject note;
    [SerializeField] private GameObject adviceText;

    private bool canRead = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            adviceText.SetActive(true);
            canRead = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            adviceText.SetActive(false);
            canRead = false;
        }
    }

    private void Update()
    {
        if (canRead)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                note.SetActive(true);
            }
        }
    }
}
