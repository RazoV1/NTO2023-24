using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConstructorObjects : MonoBehaviour
{
    [SerializeField] private List<string> ObjectsNames;
    [SerializeField] private List<GameObject> ObjectsPrefabs;

    public Transform StartPosition;
    
    public Dictionary<string, GameObject> Objects { get; private set; }
    public static ConstructorObjects Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null) Instance = this;

        Objects = new Dictionary<string, GameObject>();
        
        for (int i = 0; i < ObjectsPrefabs.Count; i++)
        {
            Objects.Add(ObjectsNames[i], ObjectsPrefabs[i]);
        }
    }
    
    
}
