using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxWithInstuments : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private GameObject AdviceText;
    private bool canUse;
    private bool used = false;

    private Inventory inventory;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !used)
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
