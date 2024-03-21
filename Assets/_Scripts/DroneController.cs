//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneController : MonoBehaviour
{
    public Transform target;

    [SerializeField] private Transform player;
    [SerializeField] private Transform backpack;
    [SerializeField] private Transform defenceAnchor;
    public GameObject shield;
    public float shieldDuration;
    public float currentShieldDuration;
    public float shieldCooldown;
    public bool isShieldActive;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Item item;
    [SerializeField] private Item battleItem;

    [SerializeField] private GameObject chargedDroneIcons;
    [SerializeField] private GameObject battleDroneItems;

    [SerializeField] private Image shieldBar;

    public List<Transform> targets;

    public int mode;

    public float speedMode1;
    public float speedMode2;
    public float speedMode3;
    public float speedCurrent;
    public Vector3 offset;
    public Vector3 battleOffset;
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

    private DronePersonalitu personas;
    
    public float moveSpeed = 2f;

    void Follow()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        if (horizontalAxis < 0) xScale = -1;
        if (horizontalAxis > 0) xScale = 1;
        Vector3 newPos = Vector3.zero;
        try
        {
            newPos = target.position;
        }
        catch
        {
            targets = new List<Transform> ();
        }
        if (mode == 1)
        {
            newPos = target.position + battleOffset;
        }
        else if (mode == 2 || mode == 4) 
        {
            newPos = target.position;
        }
        else if (mode == 3)
        {
            newPos = target.position + offset;
        }
        crest.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,new Vector3(0, 0, horizontalAxis * -30f), 100 * Time.deltaTime));
        transform.position = Vector3.Lerp(transform.position, newPos, speedCurrent * Time.deltaTime);
    }

    void ShieldUpdate()
    {
        if (currentShieldDuration <= 0.05 && isShieldActive)
        {
            personas.Appear(personas.shieldDownQuotes);
            isShieldActive = false;
            shield.SetActive(false);
            mode = 3;
            StartCoroutine(ShieldRecharge());
        }
        if (currentShieldDuration > 0 && mode == 2 && isShieldActive)
        {
            currentShieldDuration -= Time.deltaTime * 0.5f;
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);
        }
        shieldBar.fillAmount = currentShieldDuration / 5f;
    }

    private IEnumerator ShieldRecharge()
    {
        while (currentShieldDuration < shieldDuration)
        {
            currentShieldDuration += Time.deltaTime * shieldCooldown;
            yield return null;
        }
        isShieldActive = true;
        currentShieldDuration = shieldDuration;
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
                    if (Vector3.Distance(player.transform.position, i.position) <= minDis)
                    {
                        currentTarget = i;
                        minDis = Vector3.Distance(player.transform.position, i.position);
                    }
                }
                if (currentTarget.tag == "Pooh")
                {
                    if (currentTarget.GetComponentInParent<VinniePoohBehaviour>().isDead)
                    {
                        targets.Remove(currentTarget);
                        return;
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
            speedCurrent = speedMode3;
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

    private void Start()
    {
        StartCoroutine(Shot());
        personas = GetComponent<DronePersonalitu>();
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
            if (target == backpack)
            {
                crest.GetComponent<SpriteRenderer>().sortingOrder = -1;
                sprite.sortingOrder = -1;
                body.gameObject.SetActive(false);
                shotSpawnMask.gameObject.SetActive(false);
                canShoot = false;
                target = backpack;
                speedCurrent = speedMode2;
            }
            else if (target == player || mode == 3)
            {
                crest.GetComponent<SpriteRenderer>().sortingOrder = 3;
                body.gameObject.SetActive(true);
                shotSpawnMask.gameObject.SetActive(true);
                sprite.sortingOrder = 3;
                canShoot = true; 
                target = player;
                speedCurrent = speedMode1;
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
            if (mode == 1 && target != player && target != backpack)
            {
                if (Random.Range(1, 10) == 5)
                {
                    personas.Appear(personas.attackQuotes);
                }
                for (int i = 1; i <= bulletsCount; i++)
                {
                    GameObject currentBullet = Instantiate(bulletPrefab);
                    
                    currentBullet.transform.position = shotSpawnPosition.position;
                    currentBullet.GetComponent<Rigidbody>().velocity = shotSpawnPosition.transform.forward.normalized * baseBulletSpeed;
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(attackCooldown);
            }
            yield return null;
        }
    }
    private void FixedUpdate()
    {
        Follow();   
    }
    private void Update()
    {
        ShieldUpdate();
        ChooseTarget();
        LookOnCursor3D();
        ChangeTarget();
        if (player.gameObject.GetComponent<Inventory>().hasItem(battleItem))
        {
            chargedDroneIcons.SetActive(true);
            battleDroneItems.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (mode == 4)
                {
                    personas.Appear(personas.appearQuotes);
                }
                mode = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (mode == 4)
                {
                    personas.Appear(personas.appearQuotes);
                }
                else
                {
                    if (isShieldActive)
                    {
                        personas.Appear(personas.shieldUpQuotes);
                    }
                    else
                    {
                        personas.Appear(personas.shieldErrorQuotes);
                    }
                }
                if (isShieldActive)
                {
                    mode = 2;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (mode == 4)
                {
                    personas.Appear(personas.appearQuotes);
                }
                mode = 3;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (mode != 4)
                {
                    personas.Appear(personas.disappearQuotes);
                }
                mode = 4;
            }
        }
        else if (player.gameObject.GetComponent<Inventory>().hasItem(item))
        {
            chargedDroneIcons.SetActive(true);
            battleDroneItems.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (mode == 4)
                {
                    personas.Appear(personas.appearQuotes);
                }
                mode = 3;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (mode != 4)
                {
                    personas.Appear(personas.disappearQuotes);
                }
                mode = 4;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bee" || other.gameObject.tag == "Pooh" || other.gameObject.tag == "piatachok")
        {
            targets.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.tag == "Bee" || other.tag == "Pooh" || other.gameObject.tag == "piatachok")
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
    private void LookOnCursor3D()
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (mode == 3 || mode == 2)
        {
            if (Physics.Raycast(mousePos, out hit))
            {
               shotSpawnPosition.LookAt(hit.point);  
            }
        }
        else if (mode == 1)
        {
            shotSpawnPosition.LookAt(target.position);
        }
    }
}
