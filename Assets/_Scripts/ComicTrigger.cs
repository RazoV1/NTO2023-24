using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicTrigger : MonoBehaviour
{

    [SerializeField] private GameObject comic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            comic.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
