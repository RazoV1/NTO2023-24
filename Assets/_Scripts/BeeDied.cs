using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeDied : ToolBox
{
    [SerializeField] private Item item;
    [SerializeField] private GameObject AdviceText;
    [SerializeField] private bool isLore;

    [SerializeField] private List<Item> nonLoreDrops;
    public bool IsActualBee;
    
    private bool canUse;
    private bool used = false;

    private Inventory inventory;

    private TaskbarManager taskbarManager;

    private void Start()
    {
        taskbarManager = Camera.main.GetComponent<TaskbarManager>();
        if (isLore == false && IsActualBee)
        {
            item = nonLoreDrops[Random.Range(0, nonLoreDrops.Count-1)];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !used && taskbarManager.currentTask >= 3)
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
                inventory.AddItem(item, 1);

                AdviceText.SetActive(false);
            }
            
        }
    }
}
