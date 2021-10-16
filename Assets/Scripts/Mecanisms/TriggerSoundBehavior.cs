using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSoundBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            var audioRig = GetComponent<AudioRig>();
            audioRig.Play("MonsterWarning");
            var collider = GetComponent<Collider>();
            collider.enabled = !collider.enabled; 
        }

    }
}
