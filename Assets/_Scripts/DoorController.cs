using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private GameObject AdviceText;
    [SerializeField] private Door door;
    [SerializeField] private GameObject UI_door;
    [SerializeField] private GameObject closedUI;
    private bool canUse;

    [SerializeField] private GameObject closedSprite;
    [SerializeField] private GameObject openedSprite;

    public bool isBroken;
    public bool isClosed;

    
    [Header("If closed")]
    private bool canOpen = false;
    private bool oppened = false;
    [SerializeField] private GameObject lastComm;
    [SerializeField] private GameObject newComm;
    [SerializeField] private int taskToOpen;
    
    [SerializeField] private Inventory inventory;
    
    private TaskbarManager taskbarManager;
    

    private void Start()
    {
        door = GetComponent<Door>();
        if (isClosed)
        {
            closedSprite.SetActive(true);
            openedSprite.SetActive(false);
            inventory = inventory.GetComponent<Inventory>();
            taskbarManager = Camera.main.GetComponent<TaskbarManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isClosed && other.CompareTag("Player"))
        {
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
            canOpen = true;
        }
        
        else if (other.CompareTag("Player"))
        {
            
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
            canUse = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (isClosed && other.CompareTag("Player"))
        {
            AdviceText.SetActive(false);
            canOpen = false;
        }
        
        if (other.CompareTag("Player"))
        {
            AdviceText.SetActive(false);
            canUse = false;
            UI_door.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canOpen && isClosed)
            {
                closedUI.SetActive(true);
            }
            else if (canUse && !isClosed)
            {
                UI_door.SetActive(true);
            }
        }
    }

    public void UnlockDoor()
    {
        if (inventory.tryToDel(item, 1))
        {
            oppened = true;
            canOpen = false;
            taskbarManager.NextTask();
            AdviceText.SetActive(false);
            closedSprite.SetActive(false);
            openedSprite.SetActive(true);
            lastComm.SetActive(false);
            newComm.SetActive(true);
            isClosed = false;
        }
    }
}
