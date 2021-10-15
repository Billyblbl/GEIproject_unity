using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenTimer : MonoBehaviour {

	public GlobalInterface<UIController>	ui;
	public GlobalInterface<PlayerManager>	playerManager;
	public TMPro.TextMeshProUGUI	timer;

	private void Start() {
		if (!ui || !ui.currentInstance) {
			Debug.LogWarning("No ui timer");
			return;
		}
		playerManager.currentInstance.ResetGame();
		var currentTimer = Time.time - ui.currentInstance.timerStart;
		var timeSpan = System.TimeSpan.FromSeconds(currentTimer);
		timer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds/10);
		ui.currentInstance.timerRunning = false;
	}

}
