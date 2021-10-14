using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenuController : MonoBehaviour {

	public GlobalInterface<Options>	data;
	public UnityEngine.UI.Slider	masterVolumeField;
	public UnityEngine.UI.Slider	controlsSchemeField;
	public UnityEngine.UI.Slider	luminosityField;

	private void Start() {
		// Debug.Log(string.Format("Audio listener volume = {0}", AudioListener.volume));
		// Debug.Log(string.Format("master volume = {0}", data.currentInstance.masterVolume));
		masterVolumeField.value = (data.currentInstance.controlsScheme == Options.KeyboardConfig.AZERTY) ? 0 : 1;
		controlsSchemeField.value = data.currentInstance.masterVolume;
		luminosityField.value = data.currentInstance.luminosity;
	}

	public float controlsScheme { set {
		data.currentInstance.controlsScheme = (value < 0.5f) ? Options.KeyboardConfig.AZERTY : Options.KeyboardConfig.QWERTY;
		data.currentInstance.OnOptionsChanged.Trigger(data.currentInstance);
	}}

	public float masterVolume { set {
		// Debug.Log(value);
		data.currentInstance.masterVolume = value;
		data.currentInstance.OnOptionsChanged.Trigger(data.currentInstance);
	}}

	public float luminosity { set {
		data.currentInstance.luminosity = value;
		data.currentInstance.OnOptionsChanged.Trigger(data.currentInstance);
	}}
}
