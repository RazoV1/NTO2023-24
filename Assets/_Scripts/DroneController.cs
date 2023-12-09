using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public Transform target;

    [SerializeField] private Transform player;
    [SerializeField] private Transform backpack;

    [SerializeField] private SpriteRenderer sprite;

    public float speed;
    public Vector2 offset;
    public float xScale;

    private float horizontalAxis;
    private float verticalAxis;

    private bool canShoot;

    [SerializeField] private Transform crest;
    [SerializeField] private Transform body;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletsCount;
    [SerializeField] private Transform shotSpawnPosition;
    [SerializeField] private float baseBulletSpeed;

    [SerializeField] private GameObject SlimeBullet;
    
    private Vector3 mousePosition;
    public float moveSpeed = 2f;
    
    void Follow()
    {

        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (horizontalAxis < 0) xScale = -1;
        if (horizontalAxis > 0) xScale = 1;

        Vector3 newPos = target.position;
        if (target == player)
        {
            newPos.z = target.position.z;
            newPos.x = target.position.x + offset.x;
            newPos.y = target.position.y + offset.y;
        }
        
        crest.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,new Vector3(0, 0, horizontalAxis * -30f), 100 * Time.deltaTime));
        
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (target == player)
            {
                crest.GetComponent<SpriteRenderer>().sortingOrder = -1;
                sprite.sortingOrder = -1;
                body.gameObject.SetActive(false);
                canShoot = false;
                target = backpack;
                speed *= 10;
            }
            else
            {
                crest.GetComponent<SpriteRenderer>().sortingOrder = 1;
                body.gameObject.SetActive(true);
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
        currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(body.transform.forward.x, body.transform.forward.y, 0).normalized * baseBulletSpeed;
    }

    public IEnumerator Shot()
    {
        for (int i = 1; i <= bulletsCount; i++)
        {
            GameObject currentBullet = Instantiate(bulletPrefab);
            currentBullet.transform.position = shotSpawnPosition.position;

            //currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Cos(body.rotation.x) * transform.localScale.x,Mathf.Sin(body.rotation.x), 0).normalized * baseBulletSpeed;
            currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(body.transform.forward.x, body.transform.forward.y, 0).normalized * baseBulletSpeed; 
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void FixedUpdate()
    {
        Follow();
        //LookOnCursor();
        
    }
    private void Update()
    {
        LookOnCursor3D();
        ChangeTarget();
        if (Input.GetKeyDown(KeyCode.C) && canShoot)
        {
            StartCoroutine(Shot());
        }
    }

    void LookOnCursor(){ //заставляет свет следить за курсором мышки
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = body.position.z;
        mousePos.y += body.position.y - Camera.main.transform.position.y; //расстояние между камерой и объектом
        //print(mousePos);
        
        //print(mousePos);

        body.LookAt(mousePos);
    }
    private void LookOnCursor3D() //свет смотрит на точку в пространстве за курсором.
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log(mousePos);
        RaycastHit hit;
        //Debug.DrawRay(mousePos,Color.red,5);
        if (Physics.Raycast(mousePos, out hit))
        {
            body.LookAt(hit.point);
        }
    }
}
