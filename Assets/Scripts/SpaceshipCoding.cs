using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    public List<Condition> programm;
    public List<Condition> executionQueue;

    public void HideMenu()
    {
        menu.SetActive(!menu.active);
    }

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
                if (isLoaded && mainframe.allWeapons[weaponIf.weapon.value-1].MaxEnergy == mainframe.allWeapons[weaponIf.weapon.value - 1].UsingEnergy)
                {
                    //mainframe.allWeapons[weaponIf.weapon.value].Shoot();
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
        if (targetCond.oper.value == 0)
        {
            if (mainframe.parts[targetCond.checkPart.value - 1].HP >= targetCond.number)
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
            if (mainframe.parts[targetCond.checkPart.value - 1].HP <= targetCond.number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
    private void Update()
    {
        Execution();
    }
    public void AddCondition(GameObject condition)
    {
        GameObject c = Instantiate(condition,content.transform,false);
        programm.Add(c.GetComponent<Condition>());
        c.transform.position = new Vector2(c.transform.position.x,content.transform.position.y + yOffset*programm.Count);
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
            }
        }
    }
}
