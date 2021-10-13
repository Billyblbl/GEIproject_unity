using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GlobalInterface<UIController>	slot;
	public TMPro.TextMeshProUGUI			prompt;
	public TMPro.TextMeshProUGUI			timer;
	public GameObject						pauseMenu;

	public List<Image>						livesIcons;

	public int displayedLives { set {
		for (int i = 0; i < livesIcons.Count; i++)
			livesIcons[i].enabled = i < value;
	}}

	public bool timerRunning { get => _timerRunning; set {
		_timerRunning = value;
		if (!_timerRunning)
			timerPause = Time.time;
	} }

	public void TogglePauseMenu() {
		Cursor.visible = !pauseMenu.activeSelf;
		Cursor.lockState = pauseMenu.activeSelf ? CursorLockMode.Locked : CursorLockMode.Confined;
		Time.timeScale = !pauseMenu.activeSelf ? 0f : 1f;
		pauseMenu.SetActive(!pauseMenu.activeSelf);
	}

	private void Awake() { slot.currentInstance = this; }

	public void presentOnPrompt(KeyCode command, string promptText) {
		prompt.enabled = true;
		prompt.text = string.Format("({0}) : {1}", command.ToString(), promptText);
	}

	private bool _timerRunning = false;

	public void resetTimer(bool startImediately = false) {
		timerRunning = startImediately;
		timerStart = Time.time;
	}

	float timerStart = 0f;
	float timerPause = 0f;

	private void Update() {
		var currentTimer = timerRunning ? Time.time - timerStart : timerPause - timerStart;
		var timeSpan = System.TimeSpan.FromSeconds(currentTimer);
		timer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds/10);
		if (Input.GetKeyDown(KeyCode.Backspace)) {
			TogglePauseMenu();
		}
	}

}
