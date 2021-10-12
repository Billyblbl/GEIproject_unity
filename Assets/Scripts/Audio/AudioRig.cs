using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioRig : MonoBehaviour
{
    public  AudioSourceParameters[] audioSources;
    private float randomClipNumber;
    void Awake() {
       foreach (AudioSourceParameters it in audioSources)
       {
           if (!it.audioSourceObject){
                it.audioSource = gameObject.AddComponent<AudioSource>();
           }else {
               it.audioSource = it.audioSourceObject.AddComponent<AudioSource>();
           }
           it.audioSource.clip = it.audioClip;
           it.audioSource.volume = it.volume;
           it.audioSource.pitch = it.pitch;
           it.audioSource.spatialBlend = Convert.ToSingle(it.isSound3D);
           it.audioSource.minDistance = it.minDistance;
           it.audioSource.maxDistance = it.maxDistance;
           it.audioSource.dopplerLevel = it.dopplerLevel;
           it.audioSource.playOnAwake = it.playOnAwake;
       }
    }

    public void Play (string name, int index = 0)
    {
        AudioSourceParameters[] it = Array.FindAll(audioSources, audio => audio.name == name);
        it[index].audioSource.Play();
    }

    public void PlayRandom (string name)
    {
        AudioSourceParameters[] it = Array.FindAll(audioSources, audio => audio.name == name);
        it[UnityEngine.Random.Range(0, it.Length -1)].audioSource.Play();
    }
}
