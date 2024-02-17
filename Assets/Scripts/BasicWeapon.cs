using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
    public int coolDown;
    public int damage;
    public bool CanGoThroughShield;
    public int Rounds;
    public int MaxEnergy;
    public int UsingEnergy;
    public BasicPart target;
    public EnemyBehaviour enemy;
    public bool isOnCooldown;
    public bool is_automatic = true;
    [SerializeField] private Bullet bulletPrefab; 
    [SerializeField] private Transform bulletSpawnPos;
    private float timeToWait = 0;
        

    public bool is_selecting;

    public GameObject[] powers;

    private IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(coolDown);
        isOnCooldown = false;
    }
    
    public void Cycle()
    {
        if (UsingEnergy == MaxEnergy)
        {
            if (is_selecting && Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit _hit;
                Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Ray _ray = new Ray(mouseWorldPosition, Camera.main.transform.forward);
                //Получить RaycastHit2D
                Debug.DrawRay(mouseWorldPosition, Camera.main.transform.forward, Color.red,5);
                if (Physics.Raycast(_ray,out _hit))
                {
                    TrySettingTarget(_hit.transform.gameObject.GetComponent<BasicPart>());
                }
                if (target != null && !is_automatic && !isOnCooldown)
                {
                    enemy.TakeDamage(target,CanGoThroughShield,1);
                    target = null;
                    StartCoroutine(Cooldown());
                }
            }
            if (!isOnCooldown && target != null && is_automatic)
            {
                for (int i = 0; i < Rounds; i++)
                {
                    //enemy.TakeDamage(target, CanGoThroughShield, damage);
                    //print(i);
                    Bullet currentBullet = Instantiate(bulletPrefab);
                    currentBullet.transform.position = bulletSpawnPos.position + new Vector3(0, i, 0);
                    currentBullet.damage = damage;
                    currentBullet.canGoThroughShields = CanGoThroughShield;
                    currentBullet.target = target;
                    currentBullet.enemy = enemy;
                }
                StartCoroutine(Cooldown());
            }
        }
    }

    public void SetTarget()
    {
        if (is_selecting)
        {
            is_selecting = false;
        }
        else
        {
            is_selecting = true;
        }
    }
    public void TrySettingTarget(BasicPart part)
    {
        if (is_selecting && (part != target || target == null))
        {
            target = part;
            is_selecting=false;
        }
    }

    private void Update()
    {
        Cycle();
    }
}
