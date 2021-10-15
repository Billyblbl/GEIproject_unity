using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOnTrigger : MonoBehaviour
{
    [Range(0, 1000)]
    public  int     reduceFallSpeedBy;
    private bool    isFalling = false;
    private float   downSpeed = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isFalling = true;
            Destroy(gameObject, 10);
        }
    }

    private void Update() 
    {
        if (isFalling)
        {
            downSpeed += Time.deltaTime/reduceFallSpeedBy;
            transform.position = new Vector3(transform.position.x,
            transform.position.y - downSpeed,
            transform.position.z);
        }
    }

}
