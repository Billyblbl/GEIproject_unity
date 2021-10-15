using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceEnableCursorBecauseTheresABug : MonoBehaviour {

	private void Start() {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
	}
}
