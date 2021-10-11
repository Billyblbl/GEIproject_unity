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
		SceneManager.LoadScene(initalScene);
	}

	public void LoadLevel(SceneTransition transition) {
		StartCoroutine(LoadLevelAsync(transition));
	}

	public IEnumerator Fade(bool reverse = false) {
		float startTime = Time.time;
		while (Time.time - startTime < fadeTime) {
			var value = (Time.time - startTime) / fadeTime;
			fadeLayer.color = fadeTransitionGradient.Evaluate(reverse ? 1 - value : value);
			yield return null;
		}
	}

	public IEnumerator LoadLevelAsync(SceneTransition transition) {
		Debug.Log("Starting transition operation");

		if (fadeLayer)
			yield return Fade();
		else
			Debug.LogWarning("No fading layer");

		var operation = SceneManager.LoadSceneAsync(transition.target, transition.loadSceneMode);
		while (!operation.isDone) yield return null;

		if (fadeLayer)
			yield return Fade(true);

		yield return null;
	}

}
