using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float intensity;
    public float duration;
    
    public void Shake()
    {
        StartCoroutine(Explosion(intensity,duration));
    }

    private IEnumerator Explosion(float duration, float intensity)
    {
        Vector3 originalPos = Vector3.zero;
        float time = 0.0f;
        
        while (time < duration) 
        {
            transform.localPosition = new Vector3(Random.RandomRange(-1f, 1f) * intensity, Random.RandomRange(-1f, 1f) * intensity, originalPos.z);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
