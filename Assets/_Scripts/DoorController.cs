using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private GameObject AdviceText;
    [SerializeField] private Door door;
    [SerializeField] private GameObject UI_door;
    [SerializeField] private GameObject UI_broken;
    private bool canUse;

    public bool isBroken;

    private Inventory inventory;
    

    private void Start()
    {
        door = GetComponent<Door>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponent<Inventory>();
            AdviceText.SetActive(true);
            canUse = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AdviceText.SetActive(false);
            canUse = false;
            UI_door.SetActive(false);
        }
    }

    private void Update()
    {
        if (canUse)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isBroken)
                {
                    UI_broken.SetActive(true);
                }
                else
                {
                    UI_door.SetActive(true);
                }
            }
        }
    }
}
