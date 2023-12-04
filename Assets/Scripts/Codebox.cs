using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Codebox : MonoBehaviour
{
    public CodablePlatformSystem system;
    public PlayerController1 player;
    private bool canUse = false;


    private void Start()
    {
        system.UI.SetActive(false);
        player = GameObject.Find("Player").GetComponent<PlayerController1>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canUse = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canUse = false;
        }
    }

    private void Update()
    {
        if (canUse)
        {
            if (Input.GetKeyDown(KeyCode.E) && !player.is_coding)
            {
                player.is_coding = true;
                system.UI.SetActive(true);
            }
            if (player.is_coding && Input.GetKeyDown(KeyCode.Escape))
            {
                player.is_coding = false;
                system.UI.SetActive(false);
            }
        }
        else
        {
            player.is_coding = false;
            system.UI.SetActive(false);
        }
    }
}
