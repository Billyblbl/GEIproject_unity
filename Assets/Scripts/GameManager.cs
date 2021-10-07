using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public SceneReference	initalScene;

	[Header("Scene Transitions")]
	public UnityEngine.UI.Image fadeLayer = null;
	public float fadeTime = 1f;
	public Gradient fadeTransitionGradient = new Gradient();

	private void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	private void OnEnable() {
		SceneManager.LoadScene(initalScene);
	}

	public void LoadLevel(SceneTransition transition) {
		StartCoroutine(LoadLevelAsync(transition));
	}

	public IEnumerator LoadLevelAsync(SceneTransition transition) {
		Debug.Log("Starting transition operation");
		float startTime = Time.time;

		if (fadeLayer) while (Time.time - startTime < fadeTime) {
			fadeLayer.color = fadeTransitionGradient.Evaluate((Time.time - startTime) / fadeTime);
			yield return null;
		} else Debug.LogWarning("No fading layer");
		var operation = SceneManager.LoadSceneAsync(transition.target, transition.loadSceneMode);
		while (!operation.isDone) yield return null;
		startTime = Time.time;
		if (fadeLayer) while (Time.time - startTime < fadeTime) {
			fadeLayer.color = fadeTransitionGradient.Evaluate(1f - (Time.time - startTime) / fadeTime);
			yield return null;
		}
		yield return null;
	}

}
