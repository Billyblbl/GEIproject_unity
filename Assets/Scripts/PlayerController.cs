using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	[Header("Managers")]
	public GlobalInterface<UIController>	ui;
	public GlobalInterface<PlayerManager>	playerManager;

	[Header("Components")]
	public Rigidbody			rb = null;
	public Animator				animator;
	public ConfigurableJoint	lantern;

	[Header("Controls")]
	public float walkSpeed = 1f;
	public float groundCheckOffset = 0.48f;
	public float groundCheckDistance = 0.05f;
	public float jumpForce = 1f;
	public float aimSensitivity = 1f;

	[Range(-89.999999999f, 89.999999999f)] public float maxPitch = 89.999999999f;
	[Range(-89.999999999f, 89.999999999f)] public float minPitch = -89.999999999f;

	Vector2 movementInputVec = new Vector2();
	Vector2 aimInputVec = new Vector2();

	float lastGroundCheck = 0f;
	bool _grounded = false;

	private bool _alive = true;

	private Interactable	currentPromptedInterraction = null;

	//@By(billyblbl 13/10/2021@02:08)
	//@Note current implementation assumes only 1 Interactable object can be in range at once, level design MUST reflet that
	public void OnInteractableObjectInRange(Interactable obj) {
		if (ui.currentInstance) ui.currentInstance.presentOnPrompt(KeyCode.E, obj.name);
		currentPromptedInterraction = obj;
	}

	public void OnInteractableObjectOutOfRange(Interactable obj) {
		if (ui.currentInstance) ui.currentInstance.prompt.enabled = false;
		currentPromptedInterraction = null;
	}

	public bool	alive { get => _alive; set {
		_alive = value;
		animator.SetTrigger(alive ? "Reset" : "Die");
		// lantern.angularXMotion = alive ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Free;
		// lantern.angularYMotion = alive ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Free;
		// lantern.angularZMotion = alive ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Free;
		// lantern.xMotion = alive ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Free;
		// lantern.yMotion = alive ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Free;
		// lantern.zMotion = alive ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Free;
		if (lantern.connectedBody) lantern.connectedBody.transform.parent = alive ? transform : null;
		lantern.connectedBody = null;
		playerManager.currentInstance.lives--;
		// GameManager.globalInstance.Fade();
	}}
	public bool grounded { get {
		if (Time.time != lastGroundCheck)
			lastGroundCheck = Time.time;
			RaycastHit hit;
			_grounded = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, groundCheckOffset + groundCheckDistance);
		return _grounded;
	} }

	//TODO move cursor stuff in some UI manager thing
	private void Start() {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void OnDeath() {
		if (playerManager.currentInstance.lives > 0)
			playerManager.currentInstance.Respawn();
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().path, LoadSceneMode.Single);
	}

	private void Update() {
		movementInputVec.x = Input.GetAxisRaw("Horizontal");
		movementInputVec.y = Input.GetAxisRaw("Vertical");
		aimInputVec.x = Input.GetAxis("MouseX");
		aimInputVec.y = Input.GetAxis("MouseY");

		if (Input.GetKeyDown(KeyCode.Space) && grounded)
			rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

		//TODO damage system
		if (Input.GetKeyDown(KeyCode.Return)) {
			alive = false;
		}

		if (Input.GetKeyDown(KeyCode.E) && currentPromptedInterraction != null) {
			animator.SetTrigger("Interact");
			currentPromptedInterraction.OnInteract?.Invoke(currentPromptedInterraction);
		}

		if (!alive) return;

		var movementForward = transform.forward;
		movementForward.y = 0;
		movementForward.Normalize();
		var movementRight = transform.right;
		movementRight.y = 0;
		movementForward.Normalize();

		var movementVector = Vector3.ClampMagnitude(transform.right * movementInputVec.x + movementForward * movementInputVec.y, 1) * walkSpeed;
		var keptVelocity = Vector3.up * rb.velocity.y;

		animator.SetFloat("LateralVelocity", movementVector.magnitude * (grounded ? 1 : 0));

		// Debug.Log(animator.GetBool("Dead"));
		// Debug.Log(aimInputVec);

		rb.velocity = movementVector + keptVelocity;

		if (!Cursor.visible) {

			var currentPitch = Vector3.SignedAngle(movementForward, transform.forward, -transform.right);
			var newAngle = Mathf.Clamp(currentPitch + aimInputVec.y * aimSensitivity, minPitch, maxPitch);

			transform.RotateAround(transform.position, Vector3.up, aimInputVec.x * aimSensitivity);
			transform.RotateAround(transform.position, -transform.right, newAngle - currentPitch);

		}
	}

}
