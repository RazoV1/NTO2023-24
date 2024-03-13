using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyCondition : Condition
{
    public int number;
    public TMP_InputField numInp;
    public TMP_Dropdown weapon;

    private void Awake()
    {
        conditionType = "energyIf";
    }

    private void Update()
    {
        try
        {
            number = int.Parse(numInp.text);
        }
        catch
        {
            //Debug.Log("L")
        }
    }
}
