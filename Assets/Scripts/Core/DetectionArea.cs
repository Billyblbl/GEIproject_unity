using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionArea<T> : MonoBehaviour where T : MonoBehaviour {
	[SerializeField] List<T>	_detectedObjects = new List<T>();

	public List<T>			detectedObjects { get {
		_detectedObjects.RemoveAll(obj => obj == null);
		return _detectedObjects;
	} }

	public Func<T, bool>	filter = null;

	private void OnTriggerEnter(Collider other) {
		// Debug.Log("Enter " + other.name);
		// if (_detectedObjects.Find(obj => obj == other.gameObject) != null) return;
		T comp;
		if (
			!other.TryGetComponent<T>(out comp)
			|| (filter != null && !filter.Invoke(comp))
		) return;

		_detectedObjects.Add(comp);
		EnterArea?.Invoke(comp);
	}

	// private void OnTriggerStay(Collider other) {
	// 	Debug.Log(other.name);
	// }

	private void OnTriggerExit(Collider other) {
		// Debug.Log("Exit" + other.name);
		T comp;
		if (
			!other.TryGetComponent<T>(out comp)
			|| (filter != null && !filter.Invoke(comp))
		) return;
		_detectedObjects.Remove(comp);
		ExitArea?.Invoke(comp);
	}

	public UnityEvent<T>	EnterArea;
	public UnityEvent<T>	ExitArea;
	public UnityEvent<T>	StayInArea;

	private void Update() {
		_detectedObjects.RemoveAll(obj => obj == null);
		if (filter != null) foreach (var obj in _detectedObjects.FindAll(obj => !filter(obj) || !obj.GetComponent<Collider>().enabled)) {
			_detectedObjects.Remove(obj);
			ExitArea?.Invoke(obj);
		}
		foreach (var obj in _detectedObjects) StayInArea?.Invoke(obj);
	}

}