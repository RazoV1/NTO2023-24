using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomMusicCollider : MonoBehaviour
{

    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip emergencyMusic;
    [SerializeField] private AudioClip offMusic;
    [SerializeField] private Animator[] fanAnimators;
    private AudioSource soundtrack;
    

    public float oxygen;
    
    
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
        OnChange();
        soundtrack = Camera.main.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnChange();
    }

    public void ChangeRoomState(roomState _roomState)
    {
        currentRoomState = _roomState;
    }

    private void OnChange()
    {
        switch (currentRoomState)
        {
            case roomState.off:
                OffLights();
                if(soundtrack.clip == offMusic) return;
                soundtrack.clip = offMusic;
                
                return;
            
            case roomState.normal:
                NormalLightsOn();
                if(soundtrack.clip == normalMusic) return;
                soundtrack.clip = normalMusic;
                return;
            
            case roomState.emergency:
                EmergencyLightsOn();
                if(soundtrack.clip == emergencyMusic) return;
                soundtrack.clip = emergencyMusic;
                return;
            
        }
    }

    public void OffLights()
    {
        foreach (var light in lights)
        {
            light.intensity = 0;
            light.GetComponent<LightningSin>().isEmergency = false;
        }
    }

    public void EmergencyLightsOn()
    {
        foreach (var light in lights)
        {
            light.intensity = 1;
            light.color = Color.red;
            light.GetComponent<LightningSin>().isEmergency = true;
        }
    }
    
    public void NormalLightsOn()
    {
        foreach (var light in lights)
        {
            light.intensity = 3;
            light.color = Color.white;
            light.GetComponent<LightningSin>().isEmergency = false;
        }
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
