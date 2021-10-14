using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour
{    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"){
            other.transform.parent = transform;
            var animator = GetComponent<Animator>();
            animator.Play("Boat");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player"){
            other.transform.parent = null;
        }
    }
}
