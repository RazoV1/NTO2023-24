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
    private CodablePlatformSystem[] codablePlatformSystems;
    [SerializeField]private Transform gridLayoutTransform;
    [SerializeField]private GameObject buttonPrefab;

    private int doorControllersCount;
    private int tireManagersCount;
    private int lightManagersCount;
    private int doorManagersCount;
    private int codablePlatformSystemsCount;


    private void Awake()
    {
        doorControllers = GameObject.FindObjectsOfType<DoorController>();
        lightManagers = GameObject.FindObjectsOfType<LightManager>();
        tireManagers = GameObject.FindObjectsOfType<TireManager>();
        doorManagers = GameObject.FindObjectsOfType<DoorsManager>();
        codablePlatformSystems = GameObject.FindObjectsOfType<CodablePlatformSystem>();
        print(doorControllers.Length);
        print(lightManagers.Length);
        print(tireManagers.Length);
        print(doorManagers.Length);
        print(codablePlatformSystems.Length);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.P)) OpenProgrammator();
    }


    private void OpenProgrammator()
    {
        UI_Programmator.SetActive(true);
    }

    public void InstantiateAllDoorButtons()
    {
        doorControllersCount = 0;
        foreach (var doorController in doorControllers)
        {
            if (doorController.securityState == PoweredBox.SecurityState.programmator && doorController.isPowered)
            {
                doorControllersCount++; 
                GameObject currentPrefab = Instantiate(buttonPrefab, gridLayoutTransform);
                currentPrefab.GetComponent<ButtonToActive>().Ui_active = doorController.UI_door;
                currentPrefab.GetComponent<ButtonToActive>().buttonText.text = doorController.id.ToString();
            }
        }
    }
    
    public void InstantiateAllLightManagers()
    {
        lightManagersCount = 0;
        foreach (var lightManager in lightManagers)
        {
            if (lightManager.securityState == PoweredBox.SecurityState.programmator)
            {
                lightManagersCount++;
                GameObject currentPrefab = Instantiate(buttonPrefab, gridLayoutTransform);
                currentPrefab.GetComponent<ButtonToActive>().Ui_active = lightManager.UI_light;
                currentPrefab.GetComponent<ButtonToActive>().buttonText.text = lightManager.roomName;
            }
        }
    }
    
    public void InstantiateAllPlatforms()
    {
        codablePlatformSystemsCount = 0;
        foreach (var codablePlatform in codablePlatformSystems)
        {
            codablePlatformSystemsCount++;
            GameObject currentPrefab = Instantiate(buttonPrefab, gridLayoutTransform);
            currentPrefab.GetComponent<ButtonToActive>().Ui_active = codablePlatform.UI;
            currentPrefab.GetComponent<ButtonToActive>().buttonText.text = codablePlatform.platformName;
        }
    }
    
    public void InstantiateDoorManagers()
    {
        doorManagersCount = 0;
        foreach (var doorManager in doorManagers)
        {
            doorManagersCount++;
            GameObject currentPrefab = Instantiate(buttonPrefab, gridLayoutTransform);
            currentPrefab.GetComponent<ButtonToActive>().Ui_active = doorManager.UI_manager;
            currentPrefab.GetComponent<ButtonToActive>().buttonText.text = doorManager.name;
        }
    }
    
    public void InstantiateTires()
    {
        tireManagersCount = 0;
        foreach (var tireManager in tireManagers)
        {
            tireManagersCount++;
            GameObject currentPrefab = Instantiate(buttonPrefab, gridLayoutTransform);
            currentPrefab.GetComponent<ButtonToActive>().Ui_active = tireManager.UI_manager;
            currentPrefab.GetComponent<ButtonToActive>().buttonText.text = tireManager.name;
        }
    }
    
    
    public void CloseAllLightButtons()
    {
        for (int i = 0; i < lightManagersCount; i++)
        {
            Destroy(gridLayoutTransform.GetChild(i).gameObject);
        }
    }
    
    public void CloseAllTireButtons()
    {
        for (int i = 0; i < tireManagersCount; i++)
        {
            Destroy(gridLayoutTransform.GetChild(i).gameObject);
        }
    }
    
    public void CloseAllDoorManagerButtons()
    {
        for (int i = 0; i < doorManagersCount; i++)
        {
            Destroy(gridLayoutTransform.GetChild(i).gameObject);
        }
    }
    
    public void CloseAllPlatformButtons()
    {
        for (int i = 0; i < codablePlatformSystemsCount; i++)
        {
            Destroy(gridLayoutTransform.GetChild(i).gameObject);
        }
    }
    
    public void CloseAllDoorControllerButtons()
    {
        for (int i = 0; i < doorControllersCount; i++)
        {
            Destroy(gridLayoutTransform.GetChild(i).gameObject);
        }
    }

}
