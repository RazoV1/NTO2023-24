using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoneyDisplacing : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private CharacterHealth _health;
    [SerializeField] private Item honey;

    private TextMeshProUGUI text;
    
    private void Start()
    {
        _inventory = _inventory.GetComponent<Inventory>();
        _health = _health.GetComponent<CharacterHealth>();
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = _inventory.countItem(honey).ToString();
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (_inventory.tryToDel(honey, 1))
            {
                _health.TakeHeal(20f);
            }
        }

        text.text = _inventory.countItem(honey).ToString();
    }
}
