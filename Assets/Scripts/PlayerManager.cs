using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public GlobalInterface<PlayerManager>	slot = null;
	public PlayerController					playerPrefab = null;
	public PlayerController					playerEntity = null;
	public Transform 						currentCheckPoint = null;
	public GlobalInterface<UIController>	ui = null;
	[SerializeField][Range(0,3)] private int _lives = 3;
	public int lives { get => _lives; set {
		_lives = value;
		if (ui.currentInstance) ui.currentInstance.displayedLives = lives;
	}}

	public void Respawn() {
		Destroy(playerEntity.gameObject);
		playerEntity = Instantiate(playerPrefab, currentCheckPoint.position, currentCheckPoint.rotation);
	}

	private void Awake() {
		slot.currentInstance = this;
		if (!currentCheckPoint) {
			currentCheckPoint = new GameObject("player spawn point").transform;
			currentCheckPoint.position = playerEntity.transform.position;
			currentCheckPoint.rotation = playerEntity.transform.rotation;
		}
	}

	private void Start() {
		if (!ui.currentInstance) return;
		ui.currentInstance.displayedLives = lives;
		ui.currentInstance.resetTimer(true);
		ui.currentInstance.pauseMenu.gameObject.SetActive(false);
		Time.timeScale = 1f;
	}

}