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

    private float CurYOffset;

    private Vector3 currentTargetedPosition;
    public int Stingers;
    public int HP;
    public float damage;

    private Vector3 StartPos;

    private void Start()
    {
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
        Shoot();
        animator.SetTrigger("shoot");
        Stingers--;
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
        if (Stingers == 0 && canStab && Vector3.Distance(transform.position,player.transform.position) <= detectionRange)
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
                currentTargetedPosition = player.transform.position;
                UpdateOrientation(currentTargetedPosition);
                TryStab();
            }
        }

        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(transform.position, currentTargetedPosition - transform.position,Color.red,5f);
        if (!Physics.Raycast(transform.position,currentTargetedPosition - transform.position,out hit,Vector3.Distance(transform.position,currentTargetedPosition)) || hit.collider.tag == "Player")
        {
            if (Vector3.Distance(currentTargetedPosition, transform.position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, currentTargetedPosition, Time.deltaTime * speed);
                if ((Stingers == 0 && canStab) || Vector3.Distance(transform.position, player.transform.position) > detectionRange)
                {

                    currentTargetedPosition = new Vector3(Random.Range(patroolPoints[0].position.x, patroolPoints[1].position.x),
                    Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y),
                    Random.Range(patroolPoints[0].position.z, patroolPoints[1].position.z));
                    //UpdateOrientation(currentTargetedPosition);
                }
            }
            else
            {
                currentTargetedPosition = new Vector3(Random.Range(patroolPoints[0].position.x, patroolPoints[1].position.x),
                    Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y),
                    Random.Range(patroolPoints[0].position.z, patroolPoints[1].position.z));
            }
        }
        
        
        /*else
        {
            currentTargetedPosition = new Vector3(Random.Range(patroolPoints[0].position.x, patroolPoints[1].position.x),
                Random.Range(patroolPoints[0].position.y, patroolPoints[1].position.y),
                Random.Range(patroolPoints[0].position.z, patroolPoints[1].position.z));
            UpdateOrientation(currentTargetedPosition);
        }*/
    }

    private void Cycle()
    {
        RaycastHit hit;
        if (Vector3.Distance(transform.position, player.transform.position) > detectionRange)
        {
            Move();
        }
        else
        {
            UpdateOrientation(player.transform.position);
            if (Stingers > 0)
            {
                if (!canShoot)
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
