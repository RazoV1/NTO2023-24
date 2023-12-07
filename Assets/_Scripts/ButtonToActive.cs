using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonToActive : MonoBehaviour
{
    [HideInInspector] public GameObject Ui_active;
    public TextMeshProUGUI buttonText;

    public void ActiveUI()
    {
        Ui_active.SetActive(true);
    }
}
