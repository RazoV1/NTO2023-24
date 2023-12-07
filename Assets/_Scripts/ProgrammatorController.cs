using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammatorController : MonoBehaviour
{

    [SerializeField] private GameObject UI_Programmator;
    private DoorController[] doorControllers;
    private LightManager[] lightManagers;
    private TireManager[] tireManagers;
    private DoorsManager[] doorManagers;
    [SerializeField]private Transform gridLayoutTransform;
    [SerializeField]private GameObject buttonPrefab;


    private void Awake()
    {
        doorControllers = GameObject.FindObjectsOfType<DoorController>();
    }

    private void OpenProgrammator()
    {
        UI_Programmator.SetActive(true);
    }

    public void InstantiateAllDoorButtons()
    {
        foreach (var doorController in doorControllers)
        {
            if (doorController.securityState == PoweredBox.SecurityState.programmator)
            {
                GameObject currentPrefab = Instantiate(buttonPrefab, gridLayoutTransform);
                currentPrefab.GetComponent<ButtonToActive>().Ui_active = doorController.UI_door;
                currentPrefab.GetComponent<ButtonToActive>().buttonText.text = doorController.id.ToString();
            }
        }
    }
    
    
    public void CloseAllButtons()
    {
        for (int i = 0; i < doorControllers.Length; i++)
        {
            Destroy(gridLayoutTransform.GetChild(i));
        }
    }

}
