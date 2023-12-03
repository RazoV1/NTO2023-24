using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextByTrigger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textOutput;
    [SerializeField] private string text;
    public Inventory inventory;
    [SerializeField] private bool is_used;

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
