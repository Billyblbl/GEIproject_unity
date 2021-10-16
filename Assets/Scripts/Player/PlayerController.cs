using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	[Header("Managers")]
	public GlobalInterface<UIController>	ui;
	public GlobalInterface<PlayerManager>	playerManager;
	public GlobalInterface<Options>			options;

	[Header("Components")]
	public Rigidbody			rb = null;
	public Animator				animator;
	public ConfigurableJoint	lantern;
	public Transform			interactRange;
	public Camera				cam;

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

	[Header("For debug")]
	public bool	invincible = false;
	public bool enableSuicideOnEnter = false;

	private Interactable	currentPromptedInterraction = null;

	//@By(billyblbl 13/10/2021@02:08)
	//@Note current implementation assumes only 1 Interactable object can be in range at once, level design MUST reflet that
	public void OnInteractableObjectInRange(Interactable obj) {
		if (ui.currentInstance) ui.currentInstance.presentOnPrompt(KeyCode.E, obj.prompt);
		currentPromptedInterraction = obj;
		currentPromptedInterraction.OnDisableInteract.AddListener(inter => OnInteractableObjectOutOfRange(inter));
	}

	public void OnInteractableObjectOutOfRange(Interactable obj) {
		if (ui.currentInstance) ui.currentInstance.prompt.enabled = false;
		if (currentPromptedInterraction)
			currentPromptedInterraction.OnDisableInteract.RemoveAllListeners();
		currentPromptedInterraction = null;
	}

	public bool	alive { get => _alive; set {
		if (!invincible && alive != value) {
			_alive = value;
			animator.SetTrigger(alive ? "Reset" : "Die");
			if (lantern.connectedBody) lantern.connectedBody.transform.parent = alive ? transform : null;
			lantern.connectedBody = null;
			playerManager.currentInstance.lives--;
		}
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
		ui.currentInstance.prompt.enabled = false;
		if (!ui.currentInstance.timerRunning) ui.currentInstance.resetTimer(true);
		playerManager.currentInstance.playerEntity = this;
		GetComponent<Renderer>().enabled = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void OnDeath() {
		if (currentPromptedInterraction) OnInteractableObjectOutOfRange(currentPromptedInterraction);
		playerManager.currentInstance.Die(SceneManager.GetActiveScene().path);
	}

	void ScanInteractableRange() {
		var ray = new Ray(cam.transform.position, cam.transform.forward);
		RaycastHit	hit;
		Interactable interactable;
		var inRange = Physics.Raycast(ray, out hit, Vector3.Distance(interactRange.position, cam.transform.position));
		if (!inRange) {
			if (currentPromptedInterraction) OnInteractableObjectOutOfRange(currentPromptedInterraction);
		} else if (hit.collider.TryGetComponent<Interactable>(out interactable) && interactable.enabled) {
			if (interactable != currentPromptedInterraction) {
				OnInteractableObjectOutOfRange(currentPromptedInterraction);
				OnInteractableObjectInRange(interactable);
			}
		} else if (currentPromptedInterraction) OnInteractableObjectOutOfRange(currentPromptedInterraction);
	}

	private void Update() {

		ScanInteractableRange();

		movementInputVec.x = Input.GetAxisRaw(string.Format("Horizontal{0}", options.currentInstance.controlsScheme));
		movementInputVec.y = Input.GetAxisRaw(string.Format("Vertical{0}", options.currentInstance.controlsScheme));
		aimInputVec.x = Input.GetAxis("MouseX");
		aimInputVec.y = Input.GetAxis("MouseY");

		if (Input.GetKeyDown(KeyCode.Space) && grounded)
			rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

		if (enableSuicideOnEnter && Input.GetKeyDown(KeyCode.Return)) alive = false;

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
