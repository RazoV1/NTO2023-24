using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;
using Random = Unity.Mathematics.Random;

public class PiatachokGun : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform bulletSpawnTransform;
    [SerializeField] private int bulletsCount;
    [SerializeField] private float deltaBulletsAngle;
    [SerializeField] private float deltaGunAngle; //+ относительно 0
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float deltaBulletSpeed; 
    [SerializeField] private float baseBulletSpeed; 

    private void Start()
    {
        StartCoroutine(Shot());
    }
    

    public IEnumerator Shot()
    {
        pivot.rotation = UnityEngine.Quaternion.identity;
        pivot.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(0, deltaGunAngle)));
        for(int i = 1; i <= bulletsCount; i++)
        {
            GameObject currentBullet = Instantiate(bulletPrefab);
            currentBullet.transform.position = bulletSpawnTransform.position;
            currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Abs(Mathf.Cos(pivot.rotation.z + UnityEngine.Random.Range(-deltaBulletsAngle, deltaBulletsAngle))) * parent.transform.localScale.x,
                Mathf.Abs(Mathf.Abs(Mathf.Sin(pivot.rotation.z + UnityEngine.Random.Range(-deltaBulletsAngle, deltaBulletsAngle)))), 0).normalized * UnityEngine.Random.Range(baseBulletSpeed-deltaBulletSpeed, baseBulletSpeed+deltaBulletSpeed) ;
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    
}
