using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

	public string prompt;
	public UnityEvent<Interactable>	OnInteract;
	public UnityEvent<Interactable>	OnDisableInteract;

	private void OnDisable() {
		OnDisableInteract?.Invoke(this);
	}

	private void Start() {
		if (prompt == null || prompt == string.Empty) prompt = gameObject.name;
	}

}