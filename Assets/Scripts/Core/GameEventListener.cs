using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameEventListener<T> : MonoBehaviour {

	[System.Serializable]
	public struct Subscription {
		public GameEvent<T>		channel;
		public UnityEvent<T>	handler;
	}

	public List<Subscription>		subscriptions;
	List<System.Action<T>>	handlerSubs = new List<System.Action<T>>();

	private void OnEnable() {
		foreach(var sub in subscriptions) if (sub.channel) {
			// Debug.Log(string.Format("Listening on {0}", sub.channel.name));
			System.Action<T> handlerAction = data => sub.handler?.Invoke(data);
			sub.channel.hook.Add(handlerAction);
			handlerSubs.Add(handlerAction);
			// Debug.Log(string.Format("{0} currently has {1} listeners", sub.channel.name, sub.channel.hook.Count));
		}
	}

	private void OnDisable() {
		for (int i = 0; i < handlerSubs.Count; i++) if (subscriptions[i].channel)
			subscriptions[i].channel.hook.Remove(handlerSubs[i]);
	}

}