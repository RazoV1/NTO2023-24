using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogManager : MonoBehaviour
{

    public GameObject Speaker;
    public GameObject Holo;
    private DialogManager instance = null;
    void Start () {
        if (instance == null) { 
            instance = this; 
        } else if(instance == this){
            Destroy(gameObject);
        }
    }
    
    public void AnimationSpeakerTalk()
    {
        //Запускает анимацию разговора по триггеру
        Speaker.GetComponent<Animator>().SetTrigger("speak");
    }
    
    public void AnimationSpeakerIdle()
    {
        //Запускает анимацию разговора по триггеру
        Speaker.GetComponent<Animator>().SetTrigger("idle");
    }
}
