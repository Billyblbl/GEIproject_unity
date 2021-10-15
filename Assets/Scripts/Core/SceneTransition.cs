using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {
	public SceneReference	target;
	public LoadSceneMode	loadSceneMode;
	public TransitionEvent	requestChannel;
	public bool	fadeIn = true;
	public bool	fadeOut = true;

	public void request() {
		if (!requestChannel) Debug.LogWarning("No request channel for scene transition");
		requestChannel?.Trigger(this);
	}

}