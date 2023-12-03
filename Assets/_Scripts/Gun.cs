using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform shotSpawnPosition;
    [SerializeField] private GameObject waterShotPrefab;
    [SerializeField] private Vector3 right;
    [SerializeField] private Vector3 left;
    [SerializeField] private Vector3 behind;
    [SerializeField] private Vector3 forward;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform body;

    public state currentState;

    private Animator gunAnimator;

    private void Start()
    {
        gunAnimator = GetComponent<Animator>();
        playerRigidbody = playerRigidbody.GetComponent<Rigidbody>();
    }

    private bool isOpen;

    public enum state
    {
        slime,
        water,
        bubble
    }

    private void Update()
    {
        PositionChange();
        LookOnCursor();
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (currentState == state.water)
            {
                WaterShot(2);
            }
        }
    }

    private void WaterShot(float xScale)
    {
        GameObject currentShot = Instantiate(waterShotPrefab);
        currentShot.transform.position = shotSpawnPosition.position;
        currentShot.GetComponent<Rigidbody>().AddForce
            (Vector3.right * xScale * 10f, ForceMode.Impulse);
    }
    void LookOnCursor(){ //заставляет свет следить за курсором мышки
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = body.position.z;
        mousePos.y += body.position.y - Camera.main.transform.position.y; //расстояние между камерой и объектом
        //print(mousePos);
        
        print(mousePos);

        body.LookAt(mousePos);
    }
    
    private void PositionChange()
    {
        if (playerRigidbody.velocity.x > 0.1f)
        {
            transform.localPosition = left;
            transform.localScale = new Vector3(1, 1, 1);
            if (isOpen)
            {
                gunAnimator.SetTrigger("opened");
            }
            else
            {
                gunAnimator.SetTrigger("closed");
            }
        }
        
        else if (playerRigidbody.velocity.x < -0.1f)
        {
            transform.localPosition = left;
            transform.localScale = new Vector3(-1, 1, 1);
            if (isOpen)
            {
                gunAnimator.SetTrigger("opened");
            }
            else
            {
                gunAnimator.SetTrigger("closed");
            }

        }
        
        else if (playerRigidbody.velocity.z > 0.1f)
        {
            transform.localPosition = forward;
            transform.localScale = new Vector3(1, 1, 1);
            if (isOpen)
            {
                gunAnimator.SetTrigger("frontOpened");
            }
            else
            {
                gunAnimator.SetTrigger("frontClosed");
            }

        }
        else if (playerRigidbody.velocity.z < -0.1f)
        {
            transform.localPosition = behind;
            transform.localScale = new Vector3(1, 1, 1);
            if (isOpen)
            {
                gunAnimator.SetTrigger("behindOpened");
            }
            else
            {
                gunAnimator.SetTrigger("behindClosed");
            }
        }
    }
    
}
