using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Rigidbody	rb = null;

	public float walkSpeed = 1f;
	public float groundCheckOffset = 0.48f;
	public float groundCheckDistance = 0.05f;
	public float jumpForce = 1f;
	public float aimSensitivity = 1f;

	[Range(-89.999999999f, 89.999999999f)]
	public float maxPitch = 89.999999999f;

	[Range(-89.999999999f, 89.999999999f)]
	public float minPitch = -89.999999999f;

	Vector2 movementInputVec = new Vector2();
	Vector2 aimInputVec = new Vector2();

	float lastGroundCheck = 0f;
	bool _grounded = false;
	public bool grounded { get {
		if (Time.time != lastGroundCheck)
			lastGroundCheck = Time.time;
			_grounded = Physics.Raycast(transform.position + Vector3.down * groundCheckOffset, Vector3.down, groundCheckDistance);
		return _grounded;
	} }

	//TODO move cursor stuff in some UI manager thing
	private void Start() {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update() {
		movementInputVec.x = Input.GetAxisRaw("Horizontal");
		movementInputVec.y = Input.GetAxisRaw("Vertical");
		aimInputVec.x = Input.GetAxis("MouseX");
		aimInputVec.y = Input.GetAxis("MouseY");

		if (Input.GetKeyDown(KeyCode.Space) && grounded)
			rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

		var movementForward = transform.forward;
		movementForward.y = 0;
		movementForward.Normalize();
		var movementRight = transform.right;
		movementRight.y = 0;
		movementForward.Normalize();

		var movementVector = Vector3.ClampMagnitude(transform.right * movementInputVec.x + movementForward * movementInputVec.y, 1);
		var keptVelocity = Vector3.up * rb.velocity.y;

		rb.velocity = movementVector * walkSpeed + keptVelocity;

		var currentPitch = Vector3.SignedAngle(movementForward, transform.forward, -transform.right);
		var newAngle = Mathf.Clamp(currentPitch + aimInputVec.y * aimSensitivity, minPitch, maxPitch);

		transform.RotateAround(transform.position, Vector3.up, aimInputVec.x * aimSensitivity);
		transform.RotateAround(transform.position, -transform.right, newAngle - currentPitch);
	}

}
