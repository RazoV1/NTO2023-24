using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class CodablePlatformSystem : MonoBehaviour
{
    public TMP_InputField inputField;
    private string[] commands;
    [SerializeField] private Transform platform;
    [SerializeField] private float platformSpeed;
    [SerializeField] private float baseStep;
    [SerializeField] private string numArray;

    [SerializeField] private TextMeshProUGUI inpText;
    [SerializeField] private Color ErrorCollor;
    [SerializeField] private Color NormalColor;

    public bool IsCoding= false;

    public GameObject UI;
    private Vector3 targetedPosition;
    private float delay;
    private float execotionPercent = 0;

    private void Start()
    {
        targetedPosition = platform.position;
        inpText.color = NormalColor;
    }
    private IEnumerator Execute()
    {
        if (targetedPosition != null)
        {
            while (Vector3.Distance(platform.position, targetedPosition) > 0.01f)
            {
                platform.position = Vector3.Slerp(platform.position, targetedPosition, platformSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            targetedPosition = platform.position;
        }
        foreach (string i in commands)
        {
            inputField.text = "Executing... " + execotionPercent / commands.Length * 100f + "//";
            if (i == "")
            {
                execotionPercent++;
                continue;
            }
            if (i.Contains(' '))
            {
                string command = i.Split(' ')[0];
                string arg = i.Split(' ')[1];
                if (numArray.Contains(arg))
                {
                    if (command == "delay")
                    {
                        yield return new WaitForSeconds(int.Parse(arg));
                        execotionPercent++;
                        continue;
                    }
                    else if (command == "up")
                    {
                        targetedPosition.y = platform.position.y + baseStep * int.Parse(arg);
                        Debug.Log(platform.position.y + baseStep * int.Parse(arg));
                    }
                    else if (command == "down")
                    {
                        targetedPosition.y = platform.position.y - baseStep * int.Parse(arg);
                    }
                    else if (command == "right")
                    {
                        targetedPosition.x = platform.position.x + baseStep * int.Parse(arg);
                    }
                    else if (command == "left")
                    {
                        targetedPosition.x = platform.position.x - baseStep * int.Parse(arg);
                    }
                    else
                    {
                        Debug.Log("Error! Unknown command");
                        StartCoroutine(Error("Error! Unknown command"));
                        StopCoroutine(Execute());
                    }
                }
                else
                {
                    Debug.Log("Error! Invalid argument");
                    StartCoroutine(Error("Error! Invalid argument"));
                    StopCoroutine(Execute());
                }
            }
            else
            {
                Debug.Log("Error! Invalid input");
                StartCoroutine(Error("Error! Invalid input"));
                StopCoroutine(Execute());
            }
            if (targetedPosition != null)
            {
                while (Vector3.Distance(platform.position, targetedPosition) > 0.01f)
                {
                    platform.position = Vector3.Slerp(platform.position, targetedPosition, platformSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            else
            {
                targetedPosition = platform.position;
            }
            platform.position = targetedPosition;
            execotionPercent++;
            yield return null;
        }

        execotionPercent = 0;
        commands = new string[0];
        inputField.text = "Finished!";
    }

    private IEnumerator Error(string error)
    {
        inpText.color = ErrorCollor;
        inputField.text = error;
        yield return new WaitForSeconds(1);
        inpText.color = NormalColor;
        inputField.text = "";
    }

    public void FinishCode()
    {
        commands = new string[inputField.text.Split('\n').Length];
        commands = inputField.text.Split('\n');
        
        StartCoroutine(Execute());
    }
}
