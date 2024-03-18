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

    private IEnumerator IncorrectAswer()
    {
        inputField.interactable = false;
        //inputField.text = "Неправильно!";
        Color c = inputField.textComponent.color;
        inputField.textComponent.color = Color.red;
        yield return new WaitForSeconds(1f);
        inputField.interactable = true;
        inputField.textComponent.color = c;
        inputField.text = "";
    }
    private IEnumerator CorrectAnswer()
    {
        //isUsed = true;
        inputField.text = "Correct!";
        yield return new WaitForSeconds(1f);
        inputField.text = "";
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
        //Destroy(task);
        task.SetActive(false);
        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inputField.text == rightAns.ToString() && !isUsed)
            {
                StartCoroutine(CorrectAnswer());
            }
            else
            {
                StartCoroutine(IncorrectAswer());
            }
        }
    }
}
