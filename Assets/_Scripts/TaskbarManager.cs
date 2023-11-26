using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskbarManager : MonoBehaviour
{
    public TextMeshProUGUI DialogText;
    private TaskbarManager Taskbar = null;


    public string linkJson;

    [SerializeField] private GameObject closeButton;

    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private JsonObjects jsonObj;

    private List<string> phrasesList;
    private int currentPhrase = 0;
    private bool textRunning;

    //Эта переменная нужна для того, чтобы мы могли менять автоматическое определение 
    //анимации (в функции Update), на ручное в таймлайне
    //Ее поведение реализуем если надо будет, пока простаиватся без дела
    public bool customSpeakerAnim;

    void Start()
    {

        //Парсим json файл dialogA(jsonFile) в массив phrasesList 
        phrasesList = new List<string>();
        jsonObj = JsonUtility.FromJson<JsonObjects>(jsonFile.text);
        phrasesList = jsonObj.phrases;
    }
}

