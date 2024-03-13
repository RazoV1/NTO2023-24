using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRandomizer : MonoBehaviour
{
    public List<GameObject> backgrounds;

    private void Start()
    {
        backgrounds[Random.Range(0, backgrounds.Count)].SetActive(true);
    }
}
