using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour {

	[System.Serializable] public struct State {
		public Animator		animator;
		public Vector2Int	coord;
		public bool			initial;
	}

	public List<State>	states;
	public Vector2Int	dimensions;
	public UnityEvent	OnSolved;


	(Animator animator, bool state)[,]	grid;
	private void Start() {
		grid = new (Animator, bool)[dimensions.x, dimensions.y];
		foreach(var state in states) {
			grid[state.coord.x, state.coord.y] = (
				state.animator,
				state.initial
			);
			if (state.initial) state.animator.SetTrigger("Toggle");
		}
	}

	static readonly Vector2Int[] neighboors = new Vector2Int[] {
		Vector2Int.down,
		Vector2Int.left,
		Vector2Int.right,
		Vector2Int.up
	};

	public void OnLeverInteract(Interactable lever) {
		var animator = lever.GetComponent<Animator>();
		OnCellInteract(states.Find(state => state.animator == animator).coord);
	}

	public void OnCellInteract(Vector2Int cellCoord) {

		grid[cellCoord.x, cellCoord.y].animator.SetTrigger("Toggle");
		grid[cellCoord.x, cellCoord.y].state = !grid[cellCoord.x, cellCoord.y].state;
		foreach(var offset in neighboors) {
			var neighboor = cellCoord + offset;
			if (
				neighboor.x >= 0 && neighboor.x < dimensions.x &&
				neighboor.y >= 0 && neighboor.y < dimensions.y
			) {
				grid[neighboor.x, neighboor.y].animator.SetTrigger("Toggle");
				grid[neighboor.x, neighboor.y].state = !grid[neighboor.x, neighboor.y].state;
			}
		}

		{
			int count = 0;
			foreach(var cell in grid)
				count += cell.state ? 1 : 0;
			Debug.Log(string.Format("On : {0}, Off : {1}", count, grid.Length - count));
		}

		if (isSolved) OnSolved?.Invoke();

	}

	bool isSolved { get {
		foreach(var cell in grid)
			if (!cell.state) return false;
		return true;
	}}

}
