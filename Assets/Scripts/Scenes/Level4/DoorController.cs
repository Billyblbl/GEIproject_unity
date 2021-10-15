using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour {
	public Animator animator;
	public float removalTime;

	float removingStart = 0f;
	bool	removing = false;

	public void MonsterStartRemoving() {
		removing = true;
		removingStart = Time.time;
	}

	public UnityEvent	OnRemovalComplete;

	public void Close() {
		animator.SetTrigger("Close");
	}

	private void Update() {
		if (removing) {
			animator.SetFloat("MonsterOpening", Time.time - removingStart);
			if (Time.time - removingStart > removalTime) {
				animator.SetTrigger("Open");
				removing = false;
				OnRemovalComplete?.Invoke();
			}
		}
	}
}
