using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftLogic : MonoBehaviour
{
    public Inventory inventory;
    public List<Item> craftable;
    public List<Button> interactors;
    public GameObject menu;
    public bool is_opened;
    public bool can_use;

    [SerializeField] private GameObject adviceText;

    public void CraftMenu(bool c)
    {
        is_opened = c;
        menu.SetActive(is_opened);
        CheckAvailability();
    }
    private void CheckAvailability()
    {
        if (!is_opened)
        {
            return;
        }
        for (int i = 0; i < craftable.Count; i++)
        {
            if (inventory.hasItem(craftable[i].comp1) && inventory.hasItem(craftable[i].comp2))
            {
                interactors[i].interactable = true;
            }
            else
            {
                interactors[i].interactable = false;
            }
        }
    }
    public void Craft(Item item)
    {
        if (inventory.hasItem(item.comp1) && inventory.hasItem(item.comp2))
        {
            inventory.tryToDel(item.comp1, 1);
            inventory.tryToDel(item.comp2, 1);
            inventory.AddItem(item, 1);
        }
        CheckAvailability();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !can_use)
        {
            can_use = true;
            adviceText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && can_use)
        {
            can_use = false;
            is_opened = false;
            menu.SetActive(is_opened);
            adviceText.SetActive(false);
        }
    }
    private void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
    }
    private void Update()
    {
        if (can_use && Input.GetKeyDown(KeyCode.E))
        {
            CraftMenu(true);
        }
        if (can_use && Input.GetKeyDown(KeyCode.Escape))
        {
            CraftMenu(false);
        }
    }
}
