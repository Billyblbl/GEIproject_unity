using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager	globalInstance;
	public SceneReference	initalScene;

	[Header("Scene Transitions")]
	public UnityEngine.UI.Image fadeLayer = null;
	public float fadeTime = 1f;
	public Gradient fadeTransitionGradient = new Gradient();

	private void Awake() {
		DontDestroyOnLoad(gameObject);
		globalInstance = this;
	}

	private void OnEnable() {
		if (!Application.isEditor)
			SceneManager.LoadScene(initalScene);
	}

	public void LoadLevel(SceneTransition transition) {
		StartCoroutine(LoadLevelAsync(transition, transition.fadeOut, transition.fadeIn));
	}

	public void ExitGame() {
		Application.Quit();
		Debug.LogWarning("Will not quit while in editor mode");
	}

	public IEnumerator Fade(bool reverse = false) {
		float startTime = Time.unscaledTime;
		while (Time.unscaledTime - startTime < fadeTime) {
			var value = (Time.unscaledTime - startTime) / fadeTime;
			fadeLayer.color = fadeTransitionGradient.Evaluate(reverse ? 1 - value : value);
			yield return null;
		}
	}

	public IEnumerator LoadLevelAsync(SceneTransition transition, bool fadeOut, bool fadeIn) {
		Debug.Log("Starting transition operation");

		if (fadeLayer && fadeOut)
			yield return Fade();

		var operation = SceneManager.LoadSceneAsync(transition.target, transition.loadSceneMode);
		while (!operation.isDone) yield return null;

		if (fadeLayer && fadeIn)
			yield return Fade(true);

		yield return null;
	}

}
