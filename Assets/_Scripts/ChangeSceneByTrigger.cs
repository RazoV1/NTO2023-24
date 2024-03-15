using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneByTrigger : MonoBehaviour
{
    [SerializeField] private GameObject adviceText;
    [SerializeField] private int SceneIndex;

    private bool canUse = false;

    [SerializeField] private bool isLore;
    [SerializeField] private int loreTask;

    
    
    private void FixedUpdate()
    {
        if (canUse && Camera.main.GetComponent<TaskbarManager>().currentTask >= loreTask)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(SceneIndex);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canUse = true;
            adviceText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        adviceText.SetActive(false);
        canUse = false;
    }
}
