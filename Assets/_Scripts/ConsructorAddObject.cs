using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsructorAddObject : MonoBehaviour
{
    public void AddObject(string name)
    {
        GameObject prefab = Instantiate(ConstructorObjects.Instance.Objects[name]);
        prefab.transform.position = ConstructorObjects.Instance.StartPosition.position;
        prefab.transform.localScale *= 3;
    }
}
