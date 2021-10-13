using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenuController : MonoBehaviour {

	public GlobalInterface<Options>	data;
	public UnityEngine.UI.Slider	masterVolumeField;
	public UnityEngine.UI.Slider	controlsSchemeField;

	private void Start() {
		masterVolumeField.value = (data.currentInstance.controlsScheme == Options.KeyboardConfig.AZERTY) ? 0 : 1;
		controlsSchemeField.value = data.currentInstance.masterVolume;
	}

	public float controlsScheme { set {
		data.currentInstance.controlsScheme = (value < 0.5f) ? Options.KeyboardConfig.AZERTY : Options.KeyboardConfig.QWERTY;
		data.currentInstance.OnOptionsChanged.Trigger(data.currentInstance);
	}}

	public float masterVolume { set {
		data.currentInstance.masterVolume = value;
		data.currentInstance.OnOptionsChanged.Trigger(data.currentInstance);
	}}
}
