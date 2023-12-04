using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private CharacterDialog dialogCharacterText;
    [SerializeField] private List<string> phrases;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogCharacterText.GetComponent<CharacterDialog>().StartText(phrases);
            Destroy(gameObject);
        }
    }
}
