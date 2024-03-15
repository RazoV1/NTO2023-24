using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaveSin : MonoBehaviour
{
    private float sinWave;
    [SerializeField] private float multiplier;
    private Vector2 stertPos;
    public bool isE;

    private void Start()
    {
        multiplier += Random.Range(-0.05f,0.05f);
        stertPos = transform.position;
    }
    void Update()
    {
        sinWave = Mathf.Cos(Time.time);
        transform.position = new Vector2(transform.position.x, stertPos.y + sinWave * multiplier);
    }
}
