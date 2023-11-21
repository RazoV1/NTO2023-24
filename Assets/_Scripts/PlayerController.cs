using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float playerSpeed = 1f;
    [SerializeField] private Gun playerGun;
    
    private Rigidbody2D playerRigidbody;
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerGun = playerGun.GetComponent<Gun>();
    }

    private void Update()
    {
        if(Input.GetAxis("Horizontal") != 0) Move(Input.GetAxisRaw("Horizontal"));
        if (Input.GetKeyDown(KeyCode.Mouse0)) playerGun.Shot(transform.localScale.x * 18f);
    }

    private void Move(float horizontalAxis)
    {
        playerRigidbody.velocity =
            new Vector2(horizontalAxis * playerSpeed * Time.deltaTime, playerRigidbody.velocity.y);
        if (horizontalAxis > 0) transform.localScale = new Vector3(1, 1 , 1);
        else if (horizontalAxis < 0) transform.localScale = new Vector3(-1, 1 , 1);
    }
    
}
