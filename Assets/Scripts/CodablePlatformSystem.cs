using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Linq;
using UnityEditor;

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
public class CodablePlatformSystem : PoweredBox
{
    public TMP_InputField inputField;
    protected string[] commands;
    [SerializeField] protected Transform platform;
    [SerializeField] protected float platformSpeed;
    [SerializeField] protected float baseStep;
    [SerializeField] protected string numArray;
    [SerializeField] protected bool can_move = true;
    [SerializeField] protected TextMeshProUGUI inpText;
    [SerializeField] protected Color ErrorCollor;
    [SerializeField] protected Color NormalColor;

    private int commandIndex = 0;
    private int cycleSkipAmount;

    private bool isInCycle = false;
    private List<string> cycleCommands = new List<string>();
    private int CycleRepeats;
    private bool isCycleFinite = false;

    private bool isInIf = false;
    private List<string> ifCommands = new List<string>();
    private int ifRepeats;
    private bool isIfFinite = false;

    private bool ifCan_move;

    public bool IsCoding = false;
    public PointList ListOfPointLists = new PointList();
    public GameObject UI;
    protected Vector3 targetedPosition;
    protected float delay;
    protected float execotionPercent = 0;
    public string platformName;

    private void Start()
    {
        targetedPosition = platform.position;
        inpText.color = NormalColor;
        can_move = true;
    }
    protected string NewInpText()
    {
        string newS = "";
        for (int i = 0; i < inputField.text.Length - 1; i++)
        {
            newS += inputField.text[i];
        }
        return newS;
    }
    public void PrintInp(string inp)
    {
        if (inputField.text != "")
        {
            if (inputField.text.Split("\n")[inputField.text.Split("\n").Length -1].Split(" ")[0] == inp.Split(" ")[0] || (inputField.text.Split("\n").Length == 1 && inputField.text.Split(" ")[0] == inp.Split(" ")[0]))
            {
                Debug.Log("L");
                inputField.text = NewInpText() + (int.Parse(""+ inputField.text.Split("\n")[inputField.text.Split("\n").Length - 1].Split(" ")[1]) + 1).ToString();
            }
            else
            {
                Debug.Log("L2");
                inputField.text += "\n" + inp;
            }
        }
        else
        {
            Debug.Log("L3");
            inputField.text += inp;
        }
    }

    public void ClearAll()
    {
        inputField.text = null;
    }

    protected bool CanMove(Vector2 newPos)
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

    public virtual IEnumerator Execute()
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
        //Debug.Log(commands);
        
        /*foreach (string i in commands)
        {
            Debug.Log(i);
        }*/
        foreach (string i in commands)
        {
            if (cycleSkipAmount > 0)
            {
                cycleSkipAmount--;
                commandIndex++;
                Debug.Log("skippedStep");
                continue;
            }
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
                if (numArray.Contains(arg) || (command == "if"))
                {
                    Debug.Log(command);
                    if (command == "if")
                    {
                        if (arg == "canup")
                        {
                            targetedPosition.y = platform.position.y + baseStep;
                            ifCan_move = CanMove(targetedPosition);
                            targetedPosition.y = platform.position.y - baseStep;
                        }
                        else if (arg == "candown")
                        {
                            targetedPosition.y = platform.position.y - baseStep;
                            ifCan_move = CanMove(targetedPosition);
                            targetedPosition.y = platform.position.y + baseStep;
                        }
                        else if (arg == "canright")
                        {
                            targetedPosition.x = platform.position.x + baseStep;
                            ifCan_move = CanMove(targetedPosition);
                            targetedPosition.x = platform.position.x - baseStep;
                        }
                        else if (arg == "canleft")
                        {
                            targetedPosition.x = platform.position.x - baseStep;
                            ifCan_move = CanMove(targetedPosition);
                            targetedPosition.x = platform.position.x + baseStep;
                        }
                        if (ifCan_move)
                        {
                            ifRepeats = 1;
                            Debug.Log(CycleRepeats);

                            for (int j = commandIndex + 1; j < commands.Length; j++)
                            {
                                isIfFinite = false;
                                if (commands[j].Split(' ')[0] == "endif")
                                {
                                    cycleSkipAmount = ifCommands.Count;
                                    isIfFinite = true;
                                    Debug.Log(ifCommands);
                                    foreach (string c in ifCommands)
                                    {
                                        Debug.Log(c);
                                    }
                                    Debug.Log("ifed!");
                                    break;
                                }
                                ifCommands.Add(commands[j]);
                                Debug.Log("addedCommand");
                            }
                            if (!isIfFinite)
                            {
                                Debug.Log("Error! Invalid cycle");
                                execotionPercent = 0;
                                StartCoroutine(Error("Error! Invalid cycle"));
                                StopCoroutine(Execute());
                                yield return null;
                                break;
                            }
                            else
                            {
                                foreach (string j in ifCommands)
                                {
                                    if (j == "")
                                    {
                                        execotionPercent++;
                                        continue;
                                    }
                                    if (j.Contains(' '))
                                    {
                                        string Ccommand = j.Split(' ')[0];
                                        string Carg = j.Split(' ')[1];
                                        if (numArray.Contains(Carg))
                                        {
                                            if (Ccommand == "delay")
                                            {
                                                yield return new WaitForSeconds(int.Parse(Carg));
                                                execotionPercent++;
                                                continue;
                                            }
                                            else if (Ccommand == "up")
                                            {
                                                targetedPosition.y = platform.position.y + baseStep * int.Parse(Carg);
                                                can_move = CanMove(targetedPosition);
                                            }
                                            else if (Ccommand == "down")
                                            {
                                                targetedPosition.y = platform.position.y - baseStep * int.Parse(Carg);
                                                can_move = CanMove(targetedPosition);
                                            }
                                            else if (Ccommand == "right")
                                            {
                                                targetedPosition.x = platform.position.x + baseStep * int.Parse(Carg);
                                                can_move = CanMove(targetedPosition);
                                            }
                                            else if (Ccommand == "left")
                                            {
                                                targetedPosition.x = platform.position.x - baseStep * int.Parse(Carg);
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
                                    commands = new string[0];
                                    yield return null;
                                }
                            }
                            isInCycle = false;
                            ifCommands = new List<string>();
                            continue;
                        }
                        else
                        {
                            for (int j = commandIndex + 1; j < commands.Length; j++)
                            {
                                if (commands[j].Split(' ')[0] == "endif")
                                {
                                    break;
                                }
                                //ifCommands.Add(commands[j]);
                                cycleSkipAmount++;
                                Debug.Log("addedCommand");
                            }
                            execotionPercent++;
                            commandIndex++;
                            continue;
                        }
                    }   
                    if (command == "endif")
                    {
                        execotionPercent++;
                        commandIndex++;
                        continue;
                    }
                    if (command == "cycle")
                    {
                        CycleRepeats = int.Parse(arg);
                        Debug.Log(CycleRepeats);
                        
                        for (int j = commandIndex+1; j < commands.Length; j++)
                        {
                            isCycleFinite = false;
                            if (commands[j].Split(' ')[0] == "endcycle")
                            {
                                cycleSkipAmount = cycleCommands.Count;
                                isCycleFinite = true;
                                Debug.Log(cycleCommands);
                                foreach (string c in cycleCommands)
                                {
                                    Debug.Log(c);
                                }
                                Debug.Log("cycled!");
                                break;
                            }
                            cycleCommands.Add(commands[j]);
                            Debug.Log("addedCommand");
                        }
                        if (!isCycleFinite)
                        {
                            Debug.Log("Error! Invalid cycle");
                            execotionPercent = 0;
                            StartCoroutine(Error("Error! Invalid cycle"));
                            StopCoroutine(Execute());
                            yield return null;
                            break;
                        }
                        else
                        {
                            for (int rep = 0; rep < CycleRepeats; rep++)
                            {
                                foreach (string j in cycleCommands)
                                {
                                    if (j == "")
                                    {
                                        execotionPercent++;
                                        continue;
                                    }
                                    if (j.Contains(' '))
                                    {
                                        string Ccommand = j.Split(' ')[0];
                                        string Carg = j.Split(' ')[1];
                                        if (numArray.Contains(Carg))
                                        {

                                            if (Ccommand == "delay")
                                            {
                                                yield return new WaitForSeconds(int.Parse(Carg));
                                                execotionPercent++;
                                                continue;
                                            }
                                            else if (Ccommand == "up")
                                            {
                                                targetedPosition.y = platform.position.y + baseStep * int.Parse(Carg);
                                                can_move = CanMove(targetedPosition);
                                            }
                                            else if (Ccommand == "down")
                                            {
                                                targetedPosition.y = platform.position.y - baseStep * int.Parse(Carg);
                                                can_move = CanMove(targetedPosition);
                                            }
                                            else if (Ccommand == "right")
                                            {
                                                targetedPosition.x = platform.position.x + baseStep * int.Parse(Carg);
                                                can_move = CanMove(targetedPosition);
                                            }
                                            else if (Ccommand == "left")
                                            {
                                                targetedPosition.x = platform.position.x - baseStep * int.Parse(Carg);
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
                                    commands = new string[0];
                                    yield return null;
                                }
                            }
                            isInCycle = false;
                            cycleCommands = new List<string>();
                            continue;
                        }
                    }
                    if (command == "endcycle")
                    {
                        execotionPercent++;
                        commandIndex++;
                        continue;
                    }
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
                    commandIndex++;
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
            commands = new string[0];
            yield return null;
        }
        if (execotionPercent >= 1)
        {
            inputField.text = "Finished!";
        }
        execotionPercent = 0;
        commands = new string[0];
        commandIndex = 0;
    }

    protected IEnumerator Error(string error)
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
