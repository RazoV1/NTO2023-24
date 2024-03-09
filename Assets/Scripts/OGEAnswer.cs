using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OGEAnswer : MonoBehaviour
{
    public int rightAns;
    public TMP_InputField inputField;
    public GameObject task;
    public bool isUsed = false;

    private void Update()
    {
        if (inputField.text == rightAns.ToString() && !isUsed)
        {
            isUsed = true;
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
            Destroy(task);
        }
    }
}
