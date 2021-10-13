using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Options : MonoBehaviour {

	public GlobalInterface<Options>	slot;

	public enum KeyboardConfig {
		AZERTY,
		QWERTY
	};

	public float 			masterVolume = 1f;
	public KeyboardConfig	controlsScheme = KeyboardConfig.AZERTY;

	// public UnityEvent<Options>	OnOptionsChanged;
	public GameEvent<Options>	OnOptionsChanged;

	private void Awake() {
		slot.currentInstance = this;
	}

	public void OnMasterVolumeChanged() {
		AudioListener.volume = masterVolume;
	}

	// private void Update() {
	// 	Debug.Log(string.Format("master volume : {0} | controlScheme : {1}", masterVolume, controlsScheme));
	// }

}
