using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrCondition : Condition
{
    private void Awake()
    {
        conditionType = "or";
    }
}
