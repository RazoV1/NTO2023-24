using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
{
    [SerializeField] private Inventory targetInventory;
    [SerializeField] private RectTransform itemsPanel;
    [SerializeField] private RectTransform countPanel;
    [SerializeField] private TextMeshProUGUI countTextAsset;
    public List<GameObject> itemsToRedraw = new List<GameObject>();

    private void Start()
    {
        Redraw();
    }

    public void Redraw()
    {

        foreach (var redrawItem in itemsToRedraw)
        {
            Destroy(redrawItem);
        }

        itemsToRedraw.Clear();
        
        for (var i = 0; i < targetInventory.inventoryItems.Count; i++)
        {
            var item = targetInventory.inventoryItems[i];

            var icon = new GameObject(item.name+"_Icon");
            icon.transform.parent = itemsPanel;
            icon.transform.localScale = Vector3.one;
            icon.AddComponent<Image>().sprite = item.Icon;
            var textCount = Instantiate(countTextAsset);
            textCount.transform.parent = countPanel.transform;
            textCount.transform.localScale = Vector3.one;
            textCount.text = targetInventory.inventoryItemsCount[i].ToString();
            itemsToRedraw.Add(icon);
            itemsToRedraw.Add(textCount.gameObject);
            
        }
    }
}
