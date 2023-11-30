using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoorPred : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private GameObject AdviceText;
    private bool canUse;
    private bool used = false;
    [SerializeField] private GameObject lastComm;
    [SerializeField] private GameObject newComm;
    [SerializeField] private GameObject spriteNew;
    [SerializeField] private GameObject spriteOld;


    private Inventory inventory;

    //private TaskbarManager taskbarManager;

    private void Start()
    {
        //taskbarManager = Camera.main.GetComponent<TaskbarManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !used) //&& taskbarManager.currentTask >= 2
        {
            inventory = other.GetComponent<Inventory>();
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
                spriteNew.SetActive(true);
                spriteOld.SetActive(false);
                inventory.DeleteItem(item, 1);
                AdviceText.SetActive(false);
                //taskbarManager.NextTask();
                lastComm.SetActive(false);
                newComm.SetActive(true);
            }
            
        }
    }
}
