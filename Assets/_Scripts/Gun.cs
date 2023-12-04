using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private float deltaBulletSpeed; 
    [SerializeField] private float baseBulletSpeed; 
    [SerializeField] private int bulletsCount;
    private const float minSectorAngle = -0.369f;
    private const float maxSectorAngle = 0.177f;

    public state currentState;

    public bool closedYet;
    
    private Animator gunAnimator;

    private void Start()
    {
        gunAnimator = GetComponent<Animator>();
        playerRigidbody = playerRigidbody.GetComponent<Rigidbody>();
    }

    [SerializeField] private bool isClosed;

    public enum state
    {
        slime,
        water,
        bubble
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(isClosed) OpenGun();
            else CloseGun();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(Shot());
        }
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
    
    public IEnumerator Shot()
    {
        if(currentState == state.bubble){
            for (int i = 1; i <= bulletsCount; i++)
            {
                GameObject currentBullet = Instantiate(bulletPrefab);
                currentBullet.transform.position = shotSpawnPosition.position;
                
                currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Cos(body.rotation.x) * transform.localScale.x,
                        Mathf.Abs(Mathf.Sin(body.rotation.x)), 0)
                    .normalized * baseBulletSpeed;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    
    void LookOnCursor(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        mousePos.z = body.position.z;
        mousePos.y += body.position.y - Camera.main.transform.position.y; //расстояние между камерой и объектом
        if (transform.localScale.x < 0)
        {
            if (mousePos.x > transform.position.x) return;
        }
        else if (transform.localScale.x > 0)
        {
            if (mousePos.x < transform.position.x) return;
        }
        body.LookAt(mousePos);
        float x = 0;

        if (body.rotation.x <= maxSectorAngle && body.rotation.x >= minSectorAngle) // 0.177 0.369
        {
            x = parent.localScale.x * body.localEulerAngles.x;
        }
        else
        {
            x = 0;
        }

        body.rotation = Quaternion.Euler(x, 90, 0);
     }
    
    private void PositionChange()
    {
        if (playerRigidbody.velocity.x > 0.1f)
        {
            transform.localPosition = left;
            transform.localScale = new Vector3(1, 1, 1);
            if (isClosed && !closedYet)
            {
                gunAnimator.SetTrigger("close");
                closedYet = true;
            }
            else if (!isClosed && closedYet)
            {
                gunAnimator.SetTrigger("open");
                closedYet = false;
            }
            else if (!isClosed)
            {
                gunAnimator.SetTrigger("opened");
                if (isClosed && !closedYet)
                {
                    closedYet = true;
                }
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
            if (isClosed && !closedYet)
            {
                gunAnimator.SetTrigger("close");
                closedYet = true;
            }
            else if (!isClosed && closedYet)
            {
                gunAnimator.SetTrigger("open");
                closedYet = false;
            }
            if (!isClosed)
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
            if (isClosed && !closedYet)
            {
                gunAnimator.SetTrigger("frontClose");
                closedYet = true;
            }
            else if (!isClosed && closedYet)
            {
                gunAnimator.SetTrigger("frontOpen");
                closedYet = false;
            }
            if (!isClosed)
            {
                gunAnimator.SetTrigger("frontOpened");
            }
            else
            {
                gunAnimator.SetTrigger("frontClosed");
            }

        }
        if (playerRigidbody.velocity.z < -0.1f)
        {
            transform.localPosition = behind;
            transform.localScale = new Vector3(1, 1, 1);
            if (isClosed && !closedYet)
            {
                gunAnimator.SetTrigger("behindClose");
                closedYet = true;
            }
            else if (!isClosed && closedYet)
            {
                gunAnimator.SetTrigger("behindOpen");
                closedYet = false;
            }
            if (!isClosed)
            {
                gunAnimator.SetTrigger("behindOpened");
            }
            else
            {
                gunAnimator.SetTrigger("behindClosed");
            }
        }
    }

    private void CloseGun()
    {
        isClosed = true;
    }

    public void OpenGun()
    {
        isClosed = false;
    }
    
}
