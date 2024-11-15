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
	public float			luminosity = 0.25f;

	private void Start() {
		// Debug.Log(string.Format("master volume = {0}", masterVolume));
		OnMasterVolumeChanged();
		OnLuminosityChanged();
	}
	public KeyboardConfig	controlsScheme = KeyboardConfig.AZERTY;

	// public UnityEvent<Options>	OnOptionsChanged;
	public GameEvent<Options>	OnOptionsChanged;

	private void Awake() {
		slot.currentInstance = this;
	}

	public void OnMasterVolumeChanged() {
		AudioListener.volume = masterVolume;
	}

	public void OnLuminosityChanged() {
		RenderSettings.ambientLight = new Color(luminosity, luminosity, luminosity);
	}

	// private void Update() {
	// 	Debug.Log(string.Format("master volume : {0} | controlScheme : {1}", masterVolume, controlsScheme));
	// }

}
