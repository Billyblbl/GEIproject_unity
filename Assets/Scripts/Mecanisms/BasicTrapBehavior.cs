using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrapBehavior : MonoBehaviour
{
    [Range(0f, 5f)]
    public  float delay = 0;
    private void Start()
    {
        StartCoroutine(StartTrapAfterDelay());
    }

    private IEnumerator StartTrapAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        var animator = GetComponent<Animator>();
        animator.SetTrigger("Beggin");

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var player = other.GetComponent<PlayerController>();
            player.alive = false;
        }
    }

    public  void TrapAudio()
    {
        var audioManager = GetComponent<AudioRig>();
        audioManager.Play("DefaultAudio");
    }
}
