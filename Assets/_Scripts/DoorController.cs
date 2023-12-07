using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : PoweredBox
{
    [Header("Door Controller")]
    [SerializeField] private GameObject AdviceText;
    [SerializeField] public SpriteRenderer colorSprite;
    [SerializeField] private Door door;
    [SerializeField] public GameObject UI_door;
    public int id;
    private float cycleTime;
    private float currentCycleTime;
    public int currentState;

    public Item key_uni;
    
    public bool isBroken;

    private void Start()
    {
        colorSprite = colorSprite.GetComponent<SpriteRenderer>();
        door = door.GetComponent<Door>();
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
    }
    
    override protected void OnTriggerEnter(Collider other)
    {
        if (isClosed && other.CompareTag("Player"))
        {
            if (isLore)
            {
                if (Camera.main.GetComponent<TaskbarManager>().currentTask < loreTask)
                {
                    return;
                }
            }
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
            canUse = true;
        }
        
        else if (other.CompareTag("Player"))
        {
            
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
            canUse = true;
        }
    }

    override protected void OnTriggerExit(Collider other)
    {
        
        if (isClosed && other.CompareTag("Player"))
        {
            AdviceText.SetActive(false);
           
        }
        
        if (other.CompareTag("Player"))
        {
            AdviceText.SetActive(false);
            canUse = false;
            UI_door.SetActive(false);
        }
    }

    override protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isClosed && canUse)
            {
                print(1);
                UnlockBox();
            }
            
            if (canUse && !isClosed)
            {
                print(2);
                UI_door.SetActive(true);
            }
        }
    }
    
}
