using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

	public GlobalInterface<PlayerManager>	slot = null;
	public PlayerController					playerEntity = null;
	public GlobalInterface<UIController>	ui = null;

	public SceneTransition					loseScreen = null;

	public StringEvent						MenuModeChannel;
	public StringEvent						GameModeChannel;
	
	public bool playing = false;

	//Hacky stuff because i can't see playing in the options of a handler
	public void setPlaying() => playing = true;
	public void setNotPlaying() => playing = false;


	[SerializeField][Range(0,3)] private int _lives = 3;
	public int lives { get => _lives; set {
		_lives = value;
		if (ui.currentInstance) ui.currentInstance.displayedLives = lives;
	}}

	public UnityEvent<PlayerController>	OnDeath;

	[Header("Gameplay data")]
	public bool hasExitKey = false;

	public void Die(string path) {
		OnDeath?.Invoke(playerEntity);
		if (lives == 0) Lose();
		else SceneManager.LoadScene(path, LoadSceneMode.Single);
	}

	public void Lose() {
		ResetGame();
		loseScreen.request();
	}

	public void ResetGame() {
		lives = 3;
		hasExitKey = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
		ui.currentInstance.displayedLives = lives;
		ui.currentInstance.resetTimer(false);
		ui.currentInstance.pauseMenu.gameObject.SetActive(false);
		Time.timeScale = 1f;
		MenuModeChannel.Trigger("Menu");
	}

	public void Respawn() {
		Destroy(playerEntity.gameObject);
	}

	private void Awake() {
		slot.currentInstance = this;
	}

	private void Start() {
		// Respawn();
		MenuModeChannel.Trigger("Menu");
		if (!ui || !ui.currentInstance) return;
		ui.currentInstance.displayedLives = lives;
		ui.currentInstance.resetTimer(true);
		ui.currentInstance.pauseMenu.gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

}