using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
    public class AudioSourceParameters
{   public  string  name;
    [HideInInspector]
    public AudioSource audioSource;
    public GameObject  audioSourceObject;
    public  AudioClip audioClip;
    public  bool    playOnAwake;
    public  bool isSound3D;
    [Range(0f, 1f)]
    public  float   volume;
    [Range(-3f, 3f)]
    public  float   pitch;
    [Range(0f, 5f)]
    public  float   dopplerLevel;
    [Range(0f, 100f)]
    public  float   minDistance;  
    [Range(0f, 100f)]
    public  float   maxDistance;  
}
