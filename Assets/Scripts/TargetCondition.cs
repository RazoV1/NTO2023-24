using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TargetCondition : Condition
{
    public TMP_Dropdown checkPart;
    public TMP_Dropdown oper;
    public TMP_InputField numInput;
    public TMP_Dropdown targetPart;
    public TMP_Dropdown weapon;
    public int number;

    private void Update()
    {
        try
        {
            number = int.Parse(numInput.text);
        }
        catch
        {
            //Debug.Log(number);
        }
    }

    private void Awake()
    {
        conditionType = "targetIf";
    }
}
