using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehavior : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LeverUp()
    {
        animator.Play("LeverUP");

    }

    private void LeverDown()
    {
        animator.Play("LeverDown");
    }

}
