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
    [SerializeField] private Transform UsableGun;
    private const float minSectorAngle = -0.369f;
    private const float maxSectorAngle = 0.177f;

    public SpriteRenderer sprite;

    private Vector2 lastDir;

    public state currentState;

    public bool closedYet;
    
    private Animator gunAnimator;

    private void Start()
    {
        gunAnimator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
       // mousePos.z = body.position.z;
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
        float HorInp = Input.GetAxis("Horizontal");
        float VertInp = Input.GetAxis("Vertical");
        if (!isClosed)
        {
            if (HorInp < 0)
            {
                parent.transform.localScale = new Vector3(-1, 1, 1);
                UsableGun.gameObject.SetActive(true);
            }
            else if (HorInp > 0)
            {
                parent.transform.localScale = new Vector3(1, 1, 1);
                UsableGun.gameObject.SetActive(true);
            }
            else if (VertInp != 0 && Math.Abs(HorInp) !=1f)
            {
                UsableGun.gameObject.SetActive(false);
            }
        }
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            lastDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            gunAnimator.SetFloat("X", lastDir.x);
            gunAnimator.SetFloat("Y", lastDir.y);
        }
        else
        {
            gunAnimator.SetFloat("X", lastDir.x * 0.001f);
            gunAnimator.SetFloat("Y", lastDir.y * 0.001f);
        }
    }

    private void CloseGun()
    {
        isClosed = true;
        gunAnimator.SetBool("isClosed", isClosed);
        UsableGun.gameObject.SetActive(false);
    }

    public void OpenGun()
    {
        isClosed = false;
        gunAnimator.SetBool("isClosed", isClosed);
        //UsableGun.gameObject.SetActive(true);
    }
    
}
