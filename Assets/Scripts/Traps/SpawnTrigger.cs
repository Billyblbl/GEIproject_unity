using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject monster;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            monster.SetActive(true);
        }
    }
}
