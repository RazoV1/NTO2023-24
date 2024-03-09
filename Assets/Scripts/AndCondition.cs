using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndCondition : Condition
{
    private void Awake()
    {
        conditionType = "and";
    }
}
