using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public GameObject player;
    public Vector3[] patroolPoints = new Vector3[2];
    private int patroolInd = 0;
    private bool canShoot = true;
    private bool canStab = true;
    [SerializeField] private float detectionRange;
    [SerializeField] private float speed;
    [SerializeField] private float shootCD;
    [SerializeField] private float stabCD;
    [SerializeField] private float shootMinRange;
    [SerializeField] private float stabMinRange;
    [SerializeField] private float stabDamageRange;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject Stinger;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    public int Stingers;
    public int HP;
    

    private void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        patroolPoints[0] = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z - 1);
        patroolPoints[1] = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z + 1);
    }

    void Stab()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, stabDamageRange, transform.forward);
        if (hit != null)
        {
            foreach (RaycastHit hit2 in hit)
            {
                if (hit2.collider.tag == player.tag)
                {
                    Debug.Log("stabbed!");
                }
            }
        }
    }
    IEnumerator StabCooldown()
    {
        canStab = false;
        yield return new WaitForSeconds(stabCD);
        canStab = true;
    }

    private void TryStab()
    {
        if (Vector3.Distance(transform.position,player.transform.position) <= stabMinRange && canStab)
        {
            Stab();
            animator.SetTrigger("stab");
            StartCoroutine(StabCooldown());
        } 
    }

    private void Shoot()
    {
        shootingPoint.LookAt(player.transform.position);
        Instantiate(Stinger, shootingPoint.transform.position, shootingPoint.transform.rotation);
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCD);
        canShoot = true;
    }

    private void TryShooting()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= shootMinRange && canShoot)
        {
            Shoot();
            animator.SetTrigger("shoot");
            Stingers--;
            StartCoroutine(ShootCooldown());
        }
    }

    private void UpdateOrientation(Vector3 currentTarget)
    {
        if (currentTarget.x - transform.position.x > 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    private void Cycle()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > detectionRange)
        {
            UpdateOrientation(patroolPoints[patroolInd]);
            if (Vector3.Distance(transform.position, patroolPoints[patroolInd]) > 0.4)
            {
                transform.position = Vector3.Lerp(transform.position, patroolPoints[patroolInd], Time.deltaTime * speed * 0.1f);
            }
            else
            {
                if (patroolPoints.Length - patroolInd == 1)
                {
                    patroolInd = 0;
                }
                else
                {
                    patroolInd++;
                }
            }
        }
        else
        {
            UpdateOrientation(player.transform.position);
            if (Stingers > 0 && Vector3.Distance(transform.position, player.transform.position) > stabMinRange)
            {
                if (!canShoot)
                {
                    transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime);
                }
                else 
                {
                    TryShooting();
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, player.transform.position) > stabMinRange)
                {
                    transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime);
                }
                else if(canStab)
                {
                    TryStab();
                }
            }
        }
    }

    private void Update()
    {
        Cycle();
    }

}
