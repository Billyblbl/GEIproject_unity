using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

	public GameObject currentMenu { set {
		foreach(Transform child in transform)
			child.gameObject.SetActive(false);
		value.SetActive(true);
	}}

}
