﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager Instance { get; private set; }
    #endregion

    #region Unity Editor Variables
    [SerializeField] List<Sound> sounds;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        Instance = this;

        foreach (Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.name = sound.name;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.clip = sound.audioClip;
        }
    }
    #endregion

    #region SoundManager Functions
    public void PlaySound(string _audioName)
    {
        Sound sound = sounds.Find(s => s.name == _audioName);

        if (sound == null)
            Debug.Log($"Sound '{_audioName}' not found.");
        else
            sound.audioSource.Play();
    }
    #endregion
}
