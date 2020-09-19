using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
    [Range(0f, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;
    [HideInInspector] public AudioSource audioSource;
}