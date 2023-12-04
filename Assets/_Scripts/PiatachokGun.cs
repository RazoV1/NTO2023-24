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
    [SerializeField] private BoxCollider collider;
    [SerializeField] private float cd;

    private PiatachokBehaviour parentBehaviour;

    private float xScale = 1;
    
    private void Start()
    {
        parentBehaviour = parent.GetComponent<PiatachokBehaviour>();
        
    }

    private void Update()
    {
        cd -= Time.deltaTime;
    }

    public void ShotByTrigger()
    {
        if(cd <= 0) StartCoroutine(Shot());
    }
    
    public IEnumerator Shot()
    {
        cd = 4f;
        pivot.rotation = UnityEngine.Quaternion.identity;
        pivot.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(0, deltaGunAngle)));
        parentBehaviour.OnShot(new Vector3(-1 * parentBehaviour.xScale,
            0, 0).normalized);
        for(int i = 1; i <= bulletsCount; i++)
        {
            GameObject currentBullet = Instantiate(bulletPrefab);
            currentBullet.transform.position = bulletSpawnTransform.position;
            currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Abs(Mathf.Cos(pivot.rotation.z + UnityEngine.Random.Range(-deltaBulletsAngle, deltaBulletsAngle))) * parentBehaviour.xScale,
                Mathf.Abs(Mathf.Abs(Mathf.Sin(pivot.rotation.z + UnityEngine.Random.Range(-deltaBulletsAngle, deltaBulletsAngle)))), 0).normalized * UnityEngine.Random.Range(baseBulletSpeed-deltaBulletSpeed, baseBulletSpeed+deltaBulletSpeed) ;
            yield return new WaitForSeconds(bulletsCount * 0.025f);
        }
    }

}
