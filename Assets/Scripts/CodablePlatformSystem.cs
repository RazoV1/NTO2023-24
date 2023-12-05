using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

[System.Serializable]
public class Point
{
    public List<Transform> list;
}

[System.Serializable]
public class PointList
{
    
    public List<Point> list;
}
public class CodablePlatformSystem : MonoBehaviour
{
    public TMP_InputField inputField;
    private string[] commands;
    [SerializeField] private Transform platform;
    [SerializeField] private float platformSpeed;
    [SerializeField] private float baseStep;
    [SerializeField] private string numArray;
    [SerializeField] private bool can_move = true;
    [SerializeField] private TextMeshProUGUI inpText;
    [SerializeField] private Color ErrorCollor;
    [SerializeField] private Color NormalColor;
    public bool IsCoding= false;
    public PointList ListOfPointLists = new PointList();
    public GameObject UI;
    private Vector3 targetedPosition;
    private float delay;
    private float execotionPercent = 0;

    private void Start()
    {
        targetedPosition = platform.position;
        inpText.color = NormalColor;
        can_move = true;
    }

    private bool CanMove(Vector2 newPos)
    {
        foreach (Point i in ListOfPointLists.list)
        {
            Debug.Log(i.list[0].position);
            Debug.Log(i.list[1].position);
            Rect rect = new Rect(i.list[0].position, i.list[1].position);
            rect.xMax = Mathf.Max(i.list[0].position.x, i.list[1].position.x);
            rect.yMax = Mathf.Max(i.list[0].position.y, i.list[1].position.y);
            rect.xMin = Mathf.Min(i.list[0].position.x, i.list[1].position.x);
            rect.yMin = Mathf.Min(i.list[0].position.y, i.list[1].position.y);


            Debug.DrawLine(new Vector2(i.list[0].position.x, i.list[0].position.y), new Vector2(i.list[1].position.x, i.list[1].position.y),Color.red,5f);
            if (rect.Contains(newPos))
            {
                Debug.Log("EZ");
                return true;
            }
        }
        Debug.Log("L");
        return false;
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
            inputField.text = "Executing... " + execotionPercent / commands.Length * 100f + "%";
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
                        can_move = CanMove(targetedPosition);
                        
                        //Debug.Log(platform.position.y + baseStep * int.Parse(arg));
                    }
                    else if (command == "down")
                    {
                        targetedPosition.y = platform.position.y - baseStep * int.Parse(arg);
                        can_move = CanMove(targetedPosition);
                    }
                    else if (command == "right")
                    {
                        targetedPosition.x = platform.position.x + baseStep * int.Parse(arg);
                        can_move = CanMove(targetedPosition);
                    }
                    else if (command == "left")
                    {
                        targetedPosition.x = platform.position.x - baseStep * int.Parse(arg);
                        can_move = CanMove(targetedPosition);
                    }
                    else
                    {
                        Debug.Log("Error! Unknown command");
                        execotionPercent = 0;
                        StartCoroutine(Error("Error! Unknown command"));
                        StopCoroutine(Execute());
                        yield return null;
                    }

                    if (!can_move)
                    {
                        Debug.Log("Error! Cant move here and ur stupid");
                        execotionPercent = 0;
                        StartCoroutine(Error("Error! Cant move here and ur stupid"));
                        yield return null;
                        StopCoroutine(Execute());
                        
                    }
                    execotionPercent++;
                }
                else
                {
                    Debug.Log("Error! Invalid argument");
                    execotionPercent = 0;
                    StartCoroutine(Error("Error! Invalid argument"));
                    StopCoroutine(Execute());
                    yield return null;
                    
                }
            }
            else
            {
                Debug.Log("Error! Invalid input");
                execotionPercent = 0;
                StartCoroutine(Error("Error! Invalid input"));
                StopCoroutine(Execute());
                yield return null;
            }
            if (targetedPosition != null && can_move)
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
            
            yield return null;
        }

        if (execotionPercent == 1)
        {
            inputField.text = "Finished!";
        }
        execotionPercent = 0;
        commands = new string[0];
        
    }

    private IEnumerator Error(string error)
    {
        inpText.color = ErrorCollor;
        inputField.text = error;
        execotionPercent = 0;
        commands = new string[0];
        yield return new WaitForSeconds(1);
        inpText.color = NormalColor;
        inputField.text = "";
        targetedPosition = platform.position;
    }

    public void FinishCode()
    {
        commands = new string[inputField.text.Split('\n').Length];
        commands = inputField.text.Split('\n');
        
        StartCoroutine(Execute());
    }
}
