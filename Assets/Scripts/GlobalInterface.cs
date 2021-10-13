using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "GEIProject/GlobalInterface")]
public class GlobalInterface<T> : ScriptableObject {

	private	T	_currentInstance;

	public 	T	currentInstance { get => _currentInstance; set {
		_currentInstance = value;
		OnNewInstance?.Invoke(_currentInstance);
	}}

	public UnityEvent<T>	OnNewInstance;

}
