using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop = false;
    
    [HideInInspector]
    public AudioSource source;
    [Range(0f, 1f)]
    public float volume;
    [HideInInspector]
    public float initialVolume;
    [HideInInspector]
    public float pitch = 1;
}
