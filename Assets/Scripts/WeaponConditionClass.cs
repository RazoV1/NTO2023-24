using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponConditionClass : Condition
{
    public TMP_Dropdown weapon;
    public TMP_Dropdown weaponState;
    public TMP_Dropdown action;

    private void Awake()
    {
        conditionType = "weaponIf";
    }
}
