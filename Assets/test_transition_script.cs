using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_transition_script : MonoBehaviour {


	private void Update() {
		if (Input.GetKeyDown(KeyCode.T)) {
			GetComponent<SceneTransition>().request();
		}
	}
}
