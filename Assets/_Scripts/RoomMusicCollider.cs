using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
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


    public AudioSource audio_1;
    public AudioSource audio_2;
    private AudioSource _currentMainAudio;

    public AudioClip[] clips;
    private int currentClip = 0;

    public float fadeInSec = 2f;




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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(OxygenOnHealth(other));
            OnChange();
            roomNameText.text = roomName + " : " + oxygen.ToString() + "%";
            
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
        StopCoroutine(OxygenOnHealth(other));
    }

    private IEnumerator OxygenOnHealth(Collider other)
    {
        while (true)
        {
            if (oxygen <= other.GetComponent<CharacterHealth>().oxygenResistance) ;
            {
                print(((100 - oxygen) / 100) * oxygenMaxDamage);
                other.GetComponent<CharacterHealth>().TakeDamage(((100 - oxygen) / 100) * oxygenMaxDamage);
                yield return new WaitForSeconds(1f);
            }
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
        StopCoroutine(PlayFadeCoroutine());
        StartCoroutine(PlayFadeCoroutine());
    }

    private IEnumerator PlayFadeCoroutine()
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
            yield return null;
        }

        _currentMainAudio = newMainAudio;
    }

    public void OffLights()
    {
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
        StartCoroutine(OxygenCoroutine(howMany));
    }

    private IEnumerator OxygenCoroutine(float howMany)
    {
        while (true)
        {
            if (howMany == 0)
            {
                foreach (var animator in fanAnimators)
                {
                    animator.SetTrigger("off");
                }

                yield return new WaitForSeconds(1);

            }
            else
            {
                oxygen += howMany;
                oxygen = Mathf.Clamp(oxygen, 0f, 100f);
                foreach (var animator in fanAnimators)
                {
                    animator.SetTrigger("on");
                }

                yield return new WaitForSeconds(1);
            }
        }
    }
}
