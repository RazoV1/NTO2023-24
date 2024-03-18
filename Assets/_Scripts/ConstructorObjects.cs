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
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartPosition.position += Vector3.forward;
            ConsructorAddObject.Instance.prefab.transform.position += Vector3.forward;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            StartPosition.position += Vector3.right;
            ConsructorAddObject.Instance.prefab.transform.position += Vector3.right;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartPosition.position -= Vector3.forward;
            ConsructorAddObject.Instance.prefab.transform.position -= Vector3.forward;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartPosition.position -= Vector3.right;
            ConsructorAddObject.Instance.prefab.transform.position -= Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ConsructorAddObject.Instance.prefab.transform.Rotate(new Vector3(0, 90, 0));
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            ConsructorAddObject.Instance.prefab.transform.Rotate(new Vector3(0, -90, 0));
        }
        
    }
    
}
