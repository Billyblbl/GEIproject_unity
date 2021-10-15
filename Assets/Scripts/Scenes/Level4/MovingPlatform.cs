using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public Transform start;
	public Transform end;
	public Transform platform;
	public float speed = 1f;
	public float offset = 0f;
	float startTime;

	private void OnEnable() {
		startTime = Time.time;
	}

	private void Update() {
		var advance = Mathf.Sin((Time.time + offset - Mathf.PI/2 - startTime) * speed) * 0.5f + 0.5f;
		platform.position = Vector3.Lerp(start.position, end.position, advance);
	}

}
