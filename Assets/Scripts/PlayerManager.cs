using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

	public GlobalInterface<PlayerManager>	slot = null;
	public PlayerController					playerEntity = null;
	public GlobalInterface<UIController>	ui = null;

	//TODO lose screen
	public SceneTransition					backToMainMenu = null;
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
		if (lives == 0) ResetGame();
		SceneManager.LoadScene(path, LoadSceneMode.Single);
	}

	public void ResetGame() {
		lives = 3;
		hasExitKey = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		backToMainMenu.request();
	}

	public void Respawn() {
		Destroy(playerEntity.gameObject);
	}

	private void Awake() {
		slot.currentInstance = this;
	}

	private void Start() {
		// Respawn();
		if (!ui || !ui.currentInstance) return;
		ui.currentInstance.displayedLives = lives;
		ui.currentInstance.resetTimer(true);
		ui.currentInstance.pauseMenu.gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

}