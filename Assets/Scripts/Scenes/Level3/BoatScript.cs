using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour {

	public Transform	start;
	public Transform	end;
	public float		traverseTime = 120f;

	float				traverseStart;
	bool				traversing = false;

	private void Update() {

		if (Time.time > traverseStart + traverseTime)
			traversing = false;

		if (traversing) {
			var advance = Mathf.InverseLerp(traverseStart, traverseStart + traverseTime, Time.time);
			transform.position = Vector3.Lerp(start.position, end.position, advance);
		}

	}

	public void ResetBoat() {
		transform.position = start.position;
		traversing = false;
	}

	private void OnTriggerEnter(Collider other)	{
		if (other.gameObject.tag == "Player"){
			other.transform.parent = transform;
			if (!traversing) {
				traverseStart = Time.time;
				traversing = true;
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player"){
			other.transform.parent = null;
		}
	}
}
