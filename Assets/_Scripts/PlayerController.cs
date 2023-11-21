using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    
    
    [SerializeField] private float playerSpeed = 1f;

    private Rigidbody2D playerRigidbody;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private InventoryWindow inventoryWindow;
    
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        canvasManager = canvasManager.GetComponent<CanvasManager>();
        inventoryWindow = inventoryWindow.GetComponent<InventoryWindow>();
    }

    private void Update()
    {
        InputUI();
        if(Input.GetAxis("Horizontal") != 0) Move(Input.GetAxis("Horizontal"));
        //if (Input.GetKeyDown(KeyCode.Mouse0)) playerGun.Shot(transform.localScale.x * 18f);
    }

    private void Move(float horizontalAxis)
    {
        playerRigidbody.velocity =
            new Vector2(horizontalAxis * playerSpeed * Time.deltaTime, playerRigidbody.velocity.y);
        if (horizontalAxis > 0) transform.localScale = new Vector3(1, 1 , 1);
        else if (horizontalAxis < 0) transform.localScale = new Vector3(-1, 1 , 1);
    }

    private void InputUI()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(canvasManager.IsInventoryOpen()) canvasManager.CloseInventory();
            else canvasManager.OpenInventory();
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PickUpItem"))
        {
            GetComponent<Inventory>().AddItem(other.gameObject.GetComponent<PickUpItem>().pickedUpItemScriptableObject, 1);
            inventoryWindow.Redraw();
            Destroy(other.gameObject);
        }
    }
}
