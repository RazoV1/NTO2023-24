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
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public Animator SpeakerAnimator;
    public GameObject Holo;
    public TextMeshProUGUI AiText;
    public TextMeshProUGUI BearText;
    private DialogManager instance = null;
    

    public string linkJson;

    [SerializeField] private GameObject closeButton;
    
    [SerializeField] private Image emotionSpriteRenderer;
    [SerializeField] private Sprite surprisedSprite;
    [SerializeField] private Sprite disconnectedSprite;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite thoughtfulSprite;
    
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
    
    
    private bool readedSpeaker;
    private bool readedEmotion;

    private string currentSpeaker;
    public IEnumerator RunText(float pauseBetweenPhrases)
    {
        foreach (var text in phrasesList)
        {
            foreach (var letter in text)
            {
                
                if (!readedSpeaker)
                {
                    if (letter.ToString() == "a")
                    {
                        currentSpeaker = "a";
                        readedSpeaker = true;
                    }

                    else
                    {
                        currentSpeaker = "b";
                        readedSpeaker = true;
                        
                    }
                    continue;
                }

                if (currentSpeaker == "a")
                {
                    textRunning = true;
                    AiText.text += letter;
                    yield return new WaitForSeconds(0.05f);
                }
                
                else
                {
                    if (!readedEmotion)
                    {
                        if (letter.ToString() == "h")
                        {
                            emotionSpriteRenderer.sprite = happySprite;
                        }
                        else if (letter.ToString() == "t")
                        {
                            emotionSpriteRenderer.sprite = thoughtfulSprite;
                        }
                        else if (letter.ToString() == "d")
                        {
                            emotionSpriteRenderer.sprite = disconnectedSprite;
                        }
                        else if (letter.ToString() == "s")
                        {
                            emotionSpriteRenderer.sprite = surprisedSprite;
                        }

                        readedEmotion = true;
                        continue;
                    }
                    
                    textRunning = false;
                    BearText.text += letter;
                    yield return new WaitForSeconds(0.05f);
                }
            }

            textRunning = false;
            yield return new WaitForSeconds(pauseBetweenPhrases);
            readedEmotion = false;
            readedSpeaker = false;
            AiText.text = "";
            BearText.text = "";
        }
        closeButton.SetActive(true);
    }
}
