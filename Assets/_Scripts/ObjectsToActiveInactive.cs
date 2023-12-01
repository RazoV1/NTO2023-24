using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsToActiveInactive : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToActive;
    [SerializeField] private List<GameObject> objectsToInactive;


    public void ChangeActiveStateToAll()
    {
        foreach (var objectToActive in objectsToActive)
        {
            objectToActive.SetActive(true);
        }
        foreach (var objectToInactive in objectsToInactive)
        {
            objectToInactive.SetActive(false);
        }
    }

    public void ActiveObjects()
    {
        foreach (var objectToActive in objectsToActive)
        {
            objectToActive.SetActive(true);
        }
    }
    
    public void InactiveObjects()
    {
        foreach (var objectToInactive in objectsToInactive)
        {
            objectToInactive.SetActive(false);
        }
    }
    
}
