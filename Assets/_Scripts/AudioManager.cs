using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider masterSlider;

    private void Start()
    {
        audioMixer = audioMixer.GetComponent<AudioMixer>();
        OnMusicSliderChange();
    }
    private void Update()
    {
        OnMusicSliderChange();
    }
    public void OnMusicSliderChange()
    {
        audioMixer.SetFloat("MusicVolume", -80 + musicSlider.value * 100);
        audioMixer.SetFloat("MasterVolume", -80 + masterSlider.value * 100);
    }
}
