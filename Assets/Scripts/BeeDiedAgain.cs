using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeDiedAgain : ToolBox
{

    [SerializeField] private List<Item> possibleDrops;
    public bool isActualBee;
    private void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        if (isActualBee && !isLore)
        {
            item = possibleDrops[Random.Range(0, possibleDrops.Count-1)];
        }
    }
}
