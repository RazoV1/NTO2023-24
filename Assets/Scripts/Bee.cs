using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public GameObject player;
    public Transform[] patroolPoints = new Transform[2];
    private int patroolInd = 0;
    private bool canShoot = true;
    [SerializeField] private bool canStab = true;
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

    private Vector3 StartPoint;

    private float CurYOffset;

    private Vector3 currentTargetedPosition;
    public int Stingers;
    public int HP;
    public float damage;

    private bool is_waiting = false;

    private Vector3 StartPos;

    private void Start()
    {
        StartPoint = transform.position;
        currentTargetedPosition = new Vector3(Random.Range(patroolPoints[0].position.x, patroolPoints[1].position.x), Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y), Random.Range(patroolPoints[0].position.z, patroolPoints[1].position.z));
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        //patroolPoints[0] = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z - 1);
        //patroolPoints[1] = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z + 1);
        CurYOffset = Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y);
        StartPos = Vector3.zero;
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
                    player.GetComponent<CharacterHealth>().TakeDamage(damage);
                }
            }
        }
    }
    IEnumerator StabCooldown()
    {
        canStab = false;
        yield return new WaitForSeconds(stabCD + Random.Range(0f, 7.00001f));
        Debug.Log(stabCD + Random.Range(0f, 7.00001f));
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
        yield return new WaitForSeconds(shootCD + Random.Range(0f, 4.00001f));
        canShoot = true;
    }

    IEnumerator BasikWait()
    {
        is_waiting = true;
        yield return new WaitForSeconds(Random.Range(0.0001f, 2.8001f));
        is_waiting = false;
    }

    private void TryShooting()
    {
        Shoot();
        animator.SetTrigger("shoot");
        Stingers--;
        if (Stingers == 0)
        {
            speed *= 1.5f;
        }
        StartCoroutine(ShootCooldown());
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

    private void Move()
    {
        RaycastHit hit = new RaycastHit();
        if (Stingers == 0 && canStab)
        {
            if (Vector3.Distance(patroolPoints[0].position, transform.position) > Vector3.Distance(patroolPoints[0].position, patroolPoints[1].position) ||
                        Vector3.Distance(patroolPoints[1].position, transform.position) > Vector3.Distance(patroolPoints[0].position, patroolPoints[1].position) ||
                        Mathf.Max(patroolPoints[0].position.z, patroolPoints[1].position.z) - transform.position.z < 0)
            {
                currentTargetedPosition = new Vector3(Random.Range(patroolPoints[0].position.x, patroolPoints[1].position.x),
                                                      Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y),
                                                      Random.Range(patroolPoints[0].position.z, patroolPoints[1].position.z));
            }
            else if (Physics.Raycast(shootingPoint.position, player.transform.position - shootingPoint.position, out hit, Vector3.Distance(shootingPoint.position, player.transform.position),layerMask:7))
            {
                if (hit.collider.tag != "Player")
                {
                    Debug.Log("L");

                }
                else
                {
                    currentTargetedPosition = player.transform.position;
                    UpdateOrientation(currentTargetedPosition);
                    TryStab();
                }
            }
        }
        
        
        Debug.DrawRay(shootingPoint.position, currentTargetedPosition - transform.position,Color.red,5f);
        if (!is_waiting)
        {
            if (Vector3.Distance(currentTargetedPosition, transform.position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, currentTargetedPosition, Time.deltaTime * speed);
                
                /*currentTargetedPosition = new Vector3(Random.Range(patroolPoints[0].position.x, patroolPoints[1].position.x),
                Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y),
                Random.Range(patroolPoints[0].position.z, patroolPoints[1].position.z));*/
            }
            else 
            {
                
                currentTargetedPosition = new Vector3(Random.Range(patroolPoints[0].position.x, patroolPoints[1].position.x),
                    Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y),
                    Random.Range(patroolPoints[0].position.z, patroolPoints[1].position.z));
            }
        }
    }

    private void Cycle()
    {
        RaycastHit hit;
        if (Vector3.Distance(transform.position, player.transform.position) > detectionRange /* || !Physics.Raycast(shootingPoint.position, currentTargetedPosition - shootingPoint.position, out hit, Vector3.Distance(shootingPoint.position, currentTargetedPosition))*/)
        {
            Move();
        }
        else if (Physics.Raycast(shootingPoint.position, player.transform.position - shootingPoint.position, out hit, Vector3.Distance(shootingPoint.position, player.transform.position), layerMask: 7))
        {
            if (hit.collider.tag != "Player")
            {
                Move();
                return;
            }
            UpdateOrientation(player.transform.position);
            if (Stingers > 0)
            {
                if (!canShoot)
                {
                    /*if (Vector3.Distance(patroolPoints[0].position, transform.position) > Vector3.Distance(patroolPoints[0].position, patroolPoints[1].position) ||
                        Vector3.Distance(patroolPoints[1].position, transform.position) > Vector3.Distance(patroolPoints[0].position, patroolPoints[1].position) ||
                        Mathf.Max(patroolPoints[0].position.z, patroolPoints[1].position.z) - transform.position.z < 0)
                    {
                        currentTargetedPosition = new Vector3(Random.Range(patroolPoints[0].position.x, patroolPoints[1].position.x),
                                                              Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y),
                                                              Random.Range(patroolPoints[0].position.z, patroolPoints[1].position.z));
                        Move();
                    }
                    else
                    {
                        currentTargetedPosition = player.transform.position + (transform.position - player.transform.position).normalized * shootMinRange;
                        currentTargetedPosition.y = CurYOffset;
                    }*/
                    Move();
                }
                else 
                {
                    if (Vector3.Distance(patroolPoints[0].position, transform.position) > Vector3.Distance(patroolPoints[0].position, patroolPoints[1].position) ||
                        Vector3.Distance(patroolPoints[1].position, transform.position) > Vector3.Distance(patroolPoints[0].position, patroolPoints[1].position) ||
                        Mathf.Max(patroolPoints[0].position.z, patroolPoints[1].position.z) - transform.position.z < 0)
                    {
                        currentTargetedPosition = new Vector3(Random.Range(patroolPoints[0].position.x, patroolPoints[1].position.x),
                                                              Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y),
                                                              Random.Range(patroolPoints[0].position.z, patroolPoints[1].position.z));
                    }
                    else
                    {
                        currentTargetedPosition = player.transform.position + (transform.position - player.transform.position).normalized * shootMinRange;
                        currentTargetedPosition.y = CurYOffset;
                    }
                    TryShooting();
                    //StartCoroutine(BasikWait());
                    CurYOffset = Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y);
                }
                Move();
            }
            else
            {
                Move();
            }
        }
    }

    private void Update()
    {
        Cycle();
    }

}
