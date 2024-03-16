using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq.Expressions;

public class SpaceshipCoding : MonoBehaviour
{
    [Header("Player")]
    public SpaceshipMainframe mainframe;
    public EnergyStorage playerEnergy;
    [Header("Enemy")]
    public EnemyBehaviour enemyMainframe;
    public EnergyStorage enemyEnergy;
    [Header("UI")]
    public GameObject content;
    public GameObject menu;
    public float yOffset;
    public float xOffset;
    public List<Condition> programm;
    public List<Condition> executionQueue;
    public FTLRepairDrone drone;

    public void HideMenu()
    {
        menu.SetActive(!menu.active);
        if (menu.active)
        {
            Time.timeScale = 0.00000001f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    #region ConditionChecks
    bool CheckWeaponCondition(WeaponConditionClass weaponIf)
    {
        bool isLoaded;

        if (weaponIf.weaponState.value == 0)
        {
            isLoaded = true;
        }
        else
        {
            isLoaded = false;
        }
        try
        {
            if (mainframe.allWeapons[weaponIf.weapon.value - 1].isOnCooldown != isLoaded)
            {
                if (isLoaded && mainframe.allWeapons[weaponIf.weapon.value-1].MaxEnergy == mainframe.allWeapons[weaponIf.weapon.value - 1].UsingEnergy && mainframe.allWeapons[weaponIf.weapon.value - 1].target != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (IndexOutOfRangeException)
        {
            //Debug.Log("Gut))");
            return false;
        }
    }
    bool CheckTargetCondition(TargetCondition targetCond)
    {
        try
        {
            if (targetCond.oper.value == 0)
            {
                if (enemyMainframe.parts[targetCond.checkPart.value - 1].HP >= targetCond.number)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (enemyMainframe.parts[targetCond.checkPart.value - 1].HP <= targetCond.number)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch
        {
            return false;
        }
    }
    bool CheckEnergyCondition(EnergyCondition energyIf)
    {
        if (mainframe.energy.FreePoints >= energyIf.number)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool CheckRepairCondition(RepaitCondition repair)
    {
        try
        {
            if (repair.oper.value == 0)
            {
                if (mainframe.parts[repair.part1.value-1].HP > repair.number)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (mainframe.parts[repair.part1.value - 1].HP < repair.number)
                {
                    Debug.Log("t");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch
        {
            return false; 
        }
    }
    #endregion

    private void Update()
    {
        Execution();
    }
    public void AddCondition(GameObject condition)
    {
        GameObject c = Instantiate(condition,content.transform,false);
        programm.Add(c.GetComponent<Condition>());
        c.transform.position = new Vector2(c.transform.position.x + xOffset,content.transform.position.y + yOffset*programm.Count);
    }
    bool RunOneIf()
    {
        bool isIfTrue = true;
        string conditionOperator = "";
        executionQueue = new List<Condition>();
        foreach (Condition c in programm)
        {
            if (c.conditionType == "and")
            {
                conditionOperator = "and";
               // Debug.Log("and");
                continue;
            }
            else if (c.conditionType == "or")
            {
                conditionOperator = "or";
                //Debug.Log("or");
                continue;
            }
            if (c.conditionType == "weaponIf")
            {
                if (conditionOperator == "or")
                {
                    if (CheckWeaponCondition(c.GetComponent<WeaponConditionClass>()))
                    {
                        isIfTrue = true;
                        conditionOperator = "";
                        continue;
                    }
                    else if (!CheckWeaponCondition(c.GetComponent<WeaponConditionClass>()))
                    {
                        if (isIfTrue)
                        {
                            conditionOperator = "";
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (conditionOperator == "and")
                {
                    if (CheckWeaponCondition(c.GetComponent<WeaponConditionClass>()))
                    {
                        if (isIfTrue)
                        {
                            conditionOperator = "";
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (!CheckWeaponCondition(c.GetComponent<WeaponConditionClass>()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (CheckWeaponCondition(c.GetComponent<WeaponConditionClass>()))
                    {
                        isIfTrue = true;
                        conditionOperator = "";
                        continue;
                    }
                    else if (!CheckWeaponCondition(c.GetComponent<WeaponConditionClass>()))
                    {
                        isIfTrue = false;
                        conditionOperator = "";
                        continue;
                    }
                }
            }
            else if (c.conditionType == "targetIf")
            {
                TargetCondition t = c.GetComponent<TargetCondition>();
                if (conditionOperator == "or")
                {
                    if (CheckTargetCondition(t))
                    {
                        isIfTrue = true;
                        conditionOperator = "";
                        continue;
                    }
                    else if (!CheckTargetCondition(t))
                    {
                        if (isIfTrue)
                        {
                            conditionOperator = "";
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (conditionOperator == "and")
                {
                    if (CheckTargetCondition(t))
                    {
                        if (isIfTrue)
                        {
                            conditionOperator = "";
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (!CheckTargetCondition(t))
                    {
                        return false;
                    }
                }
                else
                {
                    if (CheckTargetCondition(t))
                    {
                        isIfTrue = true;
                        conditionOperator = "";
                        continue;
                    }
                    else if (!CheckTargetCondition(t))
                    {
                        isIfTrue = false;
                        conditionOperator = "";
                        continue;
                    }
                }
            }
            else if (c.conditionType == "energyIf")
            {
                EnergyCondition t = c.GetComponent<EnergyCondition>();
                if (conditionOperator == "or")
                {
                    if (CheckEnergyCondition(t))
                    {
                        isIfTrue = true;
                        conditionOperator = "";
                        continue;
                    }
                    else if (!CheckEnergyCondition(t))
                    {
                        if (isIfTrue)
                        {
                            conditionOperator = "";
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (conditionOperator == "and")
                {
                    if (CheckEnergyCondition(t))
                    {
                        if (isIfTrue)
                        {
                            conditionOperator = "";
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (!CheckEnergyCondition(t))
                    {
                        return false;
                    }
                }
                else
                {
                    if (CheckEnergyCondition(t))
                    {
                        isIfTrue = true;
                        conditionOperator = "";
                        continue;
                    }
                    else if (!CheckEnergyCondition(t))
                    {
                        isIfTrue = false;
                        conditionOperator = "";
                        continue;
                    }
                }
            }
            else if (c.conditionType == "repairIf")
            {
                RepaitCondition t = c.GetComponent<RepaitCondition>();
                if (conditionOperator == "or")
                {
                    if (CheckRepairCondition(t))
                    {
                        isIfTrue = true;
                        conditionOperator = "";
                        continue;
                    }
                    else if (!CheckRepairCondition(t))
                    {
                        if (isIfTrue)
                        {
                            conditionOperator = "";
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (conditionOperator == "and")
                {
                    if (CheckRepairCondition(t))
                    {
                        if (isIfTrue)
                        {
                            conditionOperator = "";
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (!CheckRepairCondition(t))
                    {
                        return false;
                    }
                }
                else
                {
                    if (CheckRepairCondition(t))
                    {
                        isIfTrue = true;
                        conditionOperator = "";
                        continue;
                    }
                    else if (!CheckRepairCondition(t))
                    {
                        isIfTrue = false;
                        conditionOperator = "";
                        continue;
                    }
                }
            }
        }
        return isIfTrue;
    }
    public void Execution()
    {
       if (RunOneIf())
        {
            foreach (Condition c in programm)
            {
                if (c.conditionType == "weaponIf")
                {
                    try
                    {
                        if (c.GetComponent<WeaponConditionClass>().action.value == 0 && CheckWeaponCondition(c.GetComponent<WeaponConditionClass>()))
                        {
                            mainframe.allWeapons[c.GetComponent<WeaponConditionClass>().weapon.value - 1].Shoot();
                            Debug.Log("Fire!");
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        //Debug.Log("im gut)");
                        //throw;
                    }
                }
                else if (c.conditionType == "targetIf")
                {
                    try
                    {
                        if (CheckTargetCondition(c.GetComponent<TargetCondition>()))
                        {
                            mainframe.allWeapons[c.GetComponent<TargetCondition>().weapon.value - 1].target = enemyMainframe.parts[c.GetComponent<TargetCondition>().targetPart.value-1];
                            Debug.Log("SetTarget!");
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        //Debug.Log("im gut)");
                        //throw;
                    }
                }
                else if (c.conditionType == "energyIf")
                {
                    try
                    {
                        if (CheckEnergyCondition(c.GetComponent<EnergyCondition>()))
                        {
                            
                            for (int i = 0; i < c.GetComponent<EnergyCondition>().number; i++)
                            {
                                if (c.GetComponent<EnergyCondition>().weapon.value > 6)
                                {
                                    mainframe.energy.POWERWeapon(mainframe.allWeapons[c.GetComponent<EnergyCondition>().weapon.value - 7]);
                                }
                                else
                                {
                                    mainframe.energy.POWER(mainframe.parts[c.GetComponent<EnergyCondition>().weapon.value - 1]);
                                }
                            }
                        }
                    }
                    catch
                    {
                        //Debug.Log("L");
                    }
                }
                else if (c.conditionType == "repairIf")
                {
                    try
                    {
                        if (CheckRepairCondition(c.GetComponent<RepaitCondition>()))
                        {
                            if (!drone.isFixing)
                            {
                                drone.isFixing = true;
                                drone.StartCoroutine(drone.RepairCycle(mainframe.parts[c.GetComponent<RepaitCondition>().part1.value - 1]));
                            }
                        }
                    }
                    catch
                    {
                        //D;
                    }
                }
            }
        }
    }
}
