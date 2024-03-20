using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public Transform target;

    [SerializeField] private Transform player;
    [SerializeField] private Transform backpack;
    [SerializeField] private Transform defenceAnchor;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Item item;
    [SerializeField] private Item battleItem;

    public List<Transform> targets;

    public int mode;

    public float speed;
    public Vector3 offset;
    public float xScale;
    private float horizontalAxis;
    private float verticalAxis;

    private bool canShoot = false;

    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform crest;
    [SerializeField] private Transform body;
    [SerializeField] private int bulletsCount;
    [SerializeField] private Transform shotSpawnPosition;
    [SerializeField] private Transform shotSpawnMask;
    [SerializeField] private float baseBulletSpeed;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject SlimeBullet;
    [SerializeField] private int SlimeBulletCount;
    
    //private Vector3 mousePosition;
    public float moveSpeed = 2f;

    void Follow()
    {

        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (horizontalAxis < 0) xScale = -1;
        if (horizontalAxis > 0) xScale = 1;

        Vector3 newPos = target.position;
        if (mode == 1 || mode == 3)
        {
            newPos.z = target.position.z + offset.z;
            newPos.x = target.position.x + offset.x;
            newPos.y = target.position.y + offset.y;
        }
        crest.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,new Vector3(0, 0, horizontalAxis * -30f), 100 * Time.deltaTime));
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }

    void ChooseTarget()
    {
        if (mode == 1)
        {
            if (targets.Count != 0)
            {
                float minDis = 1000;
                Transform currentTarget = null;
                foreach (var i in targets)
                {
                    if (Vector3.Distance(transform.position, i.position) <= minDis)
                    {
                        currentTarget = i;
                    }
                }
                target = currentTarget;
            }
            else
            {
                target = player;
            }
        }
        else if (mode == 2)
        {
            target = defenceAnchor;
        }
        else if (mode == 3)
        {
            target = player;
        }
        else
        {
            target = backpack;
        }
    }

    void ChangeTarget()
    {
        if (target == backpack)
        {
            if (Vector3.Distance(transform.position,backpack.position) <= 0.1f)
            {
                crest.gameObject.SetActive(false);
                sprite.gameObject.SetActive(false);
            }
        }
        else
        {
            crest.gameObject.SetActive(true);
            sprite.gameObject.SetActive(true);
        }
        if ((player.gameObject.GetComponent<Inventory>().hasItem(item) || player.gameObject.GetComponent<Inventory>().hasItem(battleItem)))
        {
            if (target == player)
            {
                crest.GetComponent<SpriteRenderer>().sortingOrder = -1;
                sprite.sortingOrder = -1;
                body.gameObject.SetActive(false);
                shotSpawnMask.gameObject.SetActive(false);
                canShoot = false;
                target = backpack;
                speed *= 10;
            }
            else if (target == backpack)
            {
                crest.GetComponent<SpriteRenderer>().sortingOrder = 1;
                body.gameObject.SetActive(true);
                shotSpawnMask.gameObject.SetActive(true);
                sprite.sortingOrder = 3;
                canShoot = true; 
                target = player;
                speed /= 10;
            }
        }
    }
    public void SlimeShot()
    {
        GameObject currentBullet = Instantiate(SlimeBullet);
        currentBullet.transform.position = shotSpawnPosition.position;
        currentBullet.GetComponent<Rigidbody>().velocity = shotSpawnPosition.transform.forward.normalized * baseBulletSpeed;
    }
    public IEnumerator Shot()
    {
        while (true)
        {
            if (mode == 1)
            {
                for (int i = 1; i <= bulletsCount; i++)
                {
                    GameObject currentBullet = Instantiate(bulletPrefab);
                    currentBullet.transform.position = shotSpawnPosition.position;
                    currentBullet.GetComponent<Rigidbody>().velocity = shotSpawnPosition.transform.forward.normalized * baseBulletSpeed;
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(attackCooldown);
            }
        }
    }
    private void FixedUpdate()
    {
        Follow();   
    }
    private void Update()
    {
        ChooseTarget();
        LookOnCursor3D();
        ChangeTarget();
        if (player.gameObject.GetComponent<Inventory>().hasItem(battleItem))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                mode = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                mode = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                mode = 3;

            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                mode = 4;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bee" || other.tag == "Pooh")
        {
            targets.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bee" || other.tag == "Pooh")
        {
            targets.Remove(other.transform);
        }
    }

    void LookOnCursor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = body.position.z;
        mousePos.y += body.position.y - Camera.main.transform.position.y;
        body.LookAt(mousePos);
    }
    private void LookOnCursor3D() //свет смотрит на точку в пространстве за курсором.
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log(mousePos);
        RaycastHit hit;
        //Debug.DrawRay(mousePos,Color.red,5);
        if (mode == 3 || mode == 2)
        {
            if (Physics.Raycast(mousePos, out hit))
            {

                if (hit.collider.tag == "Bee")
                {
                    body.LookAt(hit.collider.transform.position);
                    //shotSpawnPosition.LookAt(hit.collider.transform.position);
                }
                else
                {
                    //body.LookAt(hit.point);
                    shotSpawnPosition.LookAt(hit.point);
                    //shotSpawnMask.rotation = Quaternion.Euler(0,0,shotSpawnPosition.rotation.z);
                }
            }
        }
        else if (mode == 1)
        {
            shotSpawnPosition.LookAt(target.position);
        }
    }
}
