using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogManager : MonoBehaviour
{

    public Animator SpeakerAnimator;
    public GameObject Holo;
    public TextMeshProUGUI DialogText;
    private DialogManager instance = null;
    

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
    
    void Start () {
        if (instance == null) { 
            instance = this; 
        } else if(instance == this){
            Destroy(gameObject);
        }

        //Парсим json файл dialogA(jsonFile) в массив phrasesList 
        phrasesList = new List<string>();
        jsonObj = JsonUtility.FromJson<JsonObjects>(jsonFile.text);
        phrasesList = jsonObj.phrases;
        
        //Инициализируем компоненты
        SpeakerAnimator = SpeakerAnimator.GetComponent<Animator>();
    }

    private void Update()
    {
        //Автоматическое определение нужной анимации говорящего
        if (textRunning) SpeakerAnimator.SetTrigger("speak");
        else SpeakerAnimator.SetTrigger("idle");
    }

    //При помощи этой функции запускаем корутину с выводом текста
    public void LoadText()
    {
        if (phrasesList.Count > currentPhrase)
        {
            StartCoroutine(RunText(3f));
            currentPhrase++;
        }
    }
    
    public IEnumerator RunText(float pauseBetweenPhrases)
    {
        foreach (var text in phrasesList)
        {
            foreach (var letter in text)
            {
                textRunning = true;
                DialogText.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            textRunning = false;
            yield return new WaitForSeconds(pauseBetweenPhrases);
            DialogText.text = "";
        }
        closeButton.SetActive(true);
    }
}
