using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskbarManager : MonoBehaviour
{
    public TextMeshProUGUI TaskText;
    private TaskbarManager Taskbar = null;
    

    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private JsonObjects jsonObj;

    private List<string> tasksList;
    public int currentTask = 0;
    

    void Start()
    {

        //Парсим json файл dialogA(jsonFile) в массив phrasesList 
        tasksList = new List<string>();
        jsonObj = JsonUtility.FromJson<JsonObjects>(jsonFile.text);
        tasksList = jsonObj.phrases;
    }

    public void NextTask()
    {
        currentTask++;
        TaskText.text = tasksList[currentTask];
        
    }
}

