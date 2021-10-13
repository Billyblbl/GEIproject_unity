using System.Collections;
using UnityEngine;

public class Guillotine : MonoBehaviour
{
    [Range(0f, 5f)]
    public  float delay = 0;
    private PlayerController player;
    private AudioRig audioManager;
    private Animator animator;

    private void Awake() {
        audioManager = GetComponent<AudioRig>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(StartTrap());
    }

    IEnumerator StartTrap()
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("StartSwinging");

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.GetComponent<PlayerController>();
            player.alive = false;
        }
    }

    public  void Swing()
    {
        audioManager.Play("Swing");
    }

    public  void Reload()
    {
        audioManager.Play("Reload");
    }
}
