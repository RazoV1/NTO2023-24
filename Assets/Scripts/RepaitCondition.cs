using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.PlayerSettings;

public class RepaitCondition : Condition
{
    public int number;
    public TMP_Dropdown part1;
    public TMP_Dropdown oper;
    public TMP_InputField num;
    public TMP_Dropdown repairPart;
    
    private void Awake()
    {
        conditionType = "repairIf";
    }

    private void Update()
    {
        try
        {
            number = int.Parse(num.text);
        }
        catch
        {
            //Debug.Log("L")
        }
    }
}
