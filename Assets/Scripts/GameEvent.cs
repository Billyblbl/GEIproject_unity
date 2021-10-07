using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "GEIProject/GameEvent")]
public class GameEvent<T> : ScriptableObject {

	public List<System.Action<T>>	hook;

	private void OnEnable() {
		hook = new List<System.Action<T>>();
	}

	public void	Trigger(T data) {
		// Debug.Log(string.Format("trigger {0}, {1} listeners", name, hook.Count));
		foreach (var handler in hook) handler.Invoke(data);
	}
}