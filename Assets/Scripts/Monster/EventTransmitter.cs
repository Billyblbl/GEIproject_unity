using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTransmitter : MonoBehaviour {
	public MonsterAI AI;

	public void Spawn() {
		AI.Spawn();
	}

	public void Walk() {
		AI.Walk();
	}

	public void StartTrack() {
		AI.StartTrack();
	}

	public void Attack() {
		AI.Attack();
	}

}
