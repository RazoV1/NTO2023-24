using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoomMusicCollider : MonoBehaviour
{

    public string roomName;
    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip emergencyMusic;
    [SerializeField] private AudioClip offMusic;
    [SerializeField] private Animator[] fanAnimators;
    [SerializeField] private AudioSource[] soundtracks;

    [SerializeField] private TextMeshProUGUI roomNameText;


    private bool bool_OxygenOnHealth;
    private bool bool_AdrenalineBoost;
    
    
    public AudioSource audio_1;
    public AudioSource audio_2;
    private AudioSource _currentMainAudio;

    public AudioClip[] clips;
    private int currentClip = 0;

    private float timeInRoom;
    [SerializeField] private float V;

    public float fadeInSec = 2f;

    private float RoomK;


    public float oxygenForm;
    

    public float oxygenVelocity;


    public int tire;
    
    public float oxygen;
    [SerializeField] private float oxygenMaxDamage;


    public List<Light> lights;

    public enum roomState
    {
        off,
        emergency,
        normal
    }

    public roomState currentRoomState;

    private void Start()
    {
        oxygenVelocity = 100f / (V / 0.2f);
        print(oxygenVelocity);
        
        _currentMainAudio = audio_1;
        switch (currentRoomState)
        {
            case roomState.off:
                OffLights();
                break;
            case roomState.emergency:
                EmergencyLightsOn();
                break;
            case roomState.normal:
                NormalLightsOn();
                break;
        }
    }

    private void Update()
    {
        if(bool_OxygenOnHealth) OxygenOnHealth(GameObject.FindGameObjectWithTag("Player"));
        if(bool_AdrenalineBoost) AdrenalineBoost(GameObject.FindGameObjectWithTag("Player"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool_OxygenOnHealth = true;
            OnChange();
            roomNameText.text = roomName + " : " + oxygen.ToString() + "%";
            bool_AdrenalineBoost = true;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            roomNameText.text = roomName + ": " + oxygen.ToString() + "%";
        }
    }
    
    

    private void OnTriggerExit(Collider other)
    {
        bool_OxygenOnHealth = false;
        bool_AdrenalineBoost = false;
        StopAllCoroutines();
        OxygenChange(oxygenForm);
        timeInRoom = 0f;
    }


    private float currentWait_AdrenalineBoost;
    private void AdrenalineBoost(GameObject other)
    {
        currentWait_AdrenalineBoost -= Time.deltaTime;
        timeInRoom++;
            if (currentWait_AdrenalineBoost <= 0 && !other.GetComponent<CharacterHealth>().isUsingAdr)
            {
                other.GetComponent<CharacterHealth>().TakeAdrenaline((2 * timeInRoom - 1) * other.GetComponent<CharacterHealth>().baseAdr * RoomK);
                currentWait_AdrenalineBoost = 1f;
            }
    }


    private float currentWait_OxygenOnHealth;
    private void OxygenOnHealth(GameObject other)
    {

        currentWait_OxygenOnHealth -= Time.deltaTime;
        if (currentWait_OxygenOnHealth <= 0 && oxygen <= other.GetComponent<CharacterHealth>().oxygenResistance)
        { 
            other.GetComponent<CharacterHealth>().TakeDamage(((100 - oxygen) / 100) * oxygenMaxDamage);
            currentWait_OxygenOnHealth = 1f;
        }
    }

    public void ChangeRoomState(roomState _roomState)
    {
        currentRoomState = _roomState;
    }

    private void OnChange()
    {
        print("OnChange");
        PlayFade();
        /*switch (currentRoomState)
        {
            case roomState.off:
                //OffLights();
                if (soundtrack.clip == offMusic) return;
                soundtrack.Stop();
                soundtrack.clip = offMusic;
                soundtrack.Play();
                return;

            case roomState.normal:
                //NormalLightsOn();
                if (soundtrack.clip == normalMusic) return;
                soundtrack.Stop();
                soundtrack.clip = normalMusic;
                soundtrack.Play();
                return;

            case roomState.emergency:
                //EmergencyLightsOn();
                if (soundtrack.clip == emergencyMusic) return;
                soundtrack.Stop();
                soundtrack.clip = emergencyMusic;
                soundtrack.Play();
                return;
        }*/
    }
    

    public void PlayFade()
    {
        //StopCoroutine(PlayFadeCoroutine());
        //StartCoroutine(PlayFadeCoroutine());
        PlayFadeVoid();
    }

    private void PlayFadeVoid()
    {
        AudioSource newMainAudio = _currentMainAudio == audio_1 ? audio_2 : audio_1;
        if(currentRoomState == roomState.off) newMainAudio.clip = clips[0];
        else if(currentRoomState == roomState.emergency) newMainAudio.clip = clips[1];
        else if(currentRoomState == roomState.normal) newMainAudio.clip = clips[2];
        
        print(newMainAudio.clip);
        
        newMainAudio.volume = 0;
        newMainAudio.Play();

        float volume;
        while (true)
        {
            volume = 1f / fadeInSec * Time.deltaTime;

            _currentMainAudio.volume -= volume;
            newMainAudio.volume += volume;

            if (newMainAudio.volume >= 1) break;
            return;
        }

        _currentMainAudio = newMainAudio;
    }

    public void OffLights()
    {
        RoomK = 1;
        currentRoomState = roomState.off;
        foreach (var light in lights)
        {
            light.intensity = 0;
            light.GetComponent<LightningSin>().isEmergency = false;
        }
        OnChange();
    }

    public void EmergencyLightsOn()
    {
        RoomK = 0.25f;
        currentRoomState = roomState.emergency;
        foreach (var light in lights)
        {
            light.intensity = 1;
            light.color = Color.red;
            light.GetComponent<LightningSin>().isEmergency = true;
        }
        OnChange();
    }
    
    public void NormalLightsOn()
    {
        RoomK = -1f;
        currentRoomState = roomState.normal;
        foreach (var light in lights)
        {
            light.intensity = 3;
            light.color = Color.white;
            light.GetComponent<LightningSin>().isEmergency = false;
        }
        OnChange();
    }


    public void OxygenChange(float howMany)
    {
        StopAllCoroutines();
        OxygenVoid(howMany);
    }


    private float currentTime_OxygenVoid;
    private void OxygenVoid(float howMany)
    {
        currentTime_OxygenVoid -= Time.deltaTime;
        print("oxygenChange on : " + roomName);
        if (howMany == 0 && currentTime_OxygenVoid <= 0)
        {
            foreach (var animator in fanAnimators)
            {
                animator.SetTrigger("off");
            }

            currentTime_OxygenVoid = 1;

        }
        else if(currentTime_OxygenVoid <= 0)
        {
            oxygen += howMany;
            oxygen = Mathf.Clamp(oxygen, 0f, 100f);
            foreach (var animator in fanAnimators)
            {
                animator.SetTrigger("on");
            }

            print(roomName);
            currentTime_OxygenVoid = 1;
        }
    }
    
    
}
