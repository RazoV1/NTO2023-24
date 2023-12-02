using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMusicCollider : MonoBehaviour
{

    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip emergencyMusic;
    [SerializeField] private AudioClip offMusic;
    private AudioSource soundtrack;

    public enum roomState
    {
        off,
        emergency,
        normal
    }

    public roomState currentRoomState;

    private void Start()
    {
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
                if(soundtrack.clip == offMusic) return;
                soundtrack.clip = offMusic;
                return;
            
            case roomState.normal:
                if(soundtrack.clip == normalMusic) return;
                soundtrack.clip = normalMusic;
                return;
            
            case roomState.emergency:
                if(soundtrack.clip == emergencyMusic) return;
                soundtrack.clip = emergencyMusic;
                return;
            
        }
    }
}
