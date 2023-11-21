using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform shotSpawnPosition;
    [SerializeField] private GameObject waterShotPrefab;
    
    public void Shot(float xScale)
    {
        GameObject currentShot = Instantiate(waterShotPrefab);
        currentShot.transform.position = shotSpawnPosition.position;
        currentShot.GetComponent<Rigidbody2D>().AddForce(new Vector2(currentShot.transform.position.x + 1 * xScale, currentShot.transform.position.y + 1f), ForceMode2D.Impulse);
    }
}
