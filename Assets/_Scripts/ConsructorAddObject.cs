using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsructorAddObject : MonoBehaviour
{
    public static ConsructorAddObject Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject prefab;
    public void AddObject(string name)
    {
        prefab = Instantiate(ConstructorObjects.Instance.Objects[name]);
        prefab.transform.position = ConstructorObjects.Instance.StartPosition.position;
        prefab.transform.localScale *= 3;
    }
}
