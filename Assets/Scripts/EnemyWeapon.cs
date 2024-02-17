using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int coolDown;
    public int damage;
    public bool CanGoThroughShield;
    public int Rounds;
    public int MaxEnergy;
    public int UsingEnergy;
    public BasicPart target;
    public SpaceshipMainframe playerSpaceship;
    public bool isOnCooldown;
    [SerializeField] private EnemyBullet bulletPrefab; 
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
            if (!isOnCooldown)
            {
                target = playerSpaceship.parts[Random.Range(0, playerSpaceship.parts.Length)];
                for (int i = 0; i < Rounds; i++)
                {
                    //enemy.TakeDamage(target, CanGoThroughShield, damage);
                    //print(i);
                    EnemyBullet currentBullet = Instantiate(bulletPrefab);
                    currentBullet.transform.position = bulletSpawnPos.position + new Vector3(0, i, 0);
                    currentBullet.damage = damage;
                    currentBullet.canGoThroughShields = CanGoThroughShield;
                    currentBullet.target = target;
                    currentBullet.playerSpaceship = playerSpaceship;
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