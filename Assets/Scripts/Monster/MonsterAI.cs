using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class MonsterAI : MonoBehaviour
{

	public GlobalInterface<PlayerManager> playerManager;
	// [SerializeField] private Transform movePositionTransform;
	private NavMeshAgent navMeshAgent;
	// public ThirdPersonCharacter character;
	public Animator animator;
	public Animator rootAnimator;
	// private PlayerController player;
	private AudioRig audioManager;
	private float lastAttack;
	[SerializeField] private float attackCooldown = 2;

	public float attackAimAngularSpeed = 1f;

	bool attacking = false;

	public void OnPlayerInRange() {
		Debug.Log("In range");
		// character.Move(Vector3.zero, false, false);
		attacking = true;
		navMeshAgent.destination = transform.position;
	}

	public void OnPlayerOutOfRange() {
		Debug.Log("Out of range");
		attacking = false;
	}

	private void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent>();
		// animator = GetComponent<Animator>();
		audioManager = GetComponent<AudioRig>();
	}


	float initialSpeed;
	public float offMeshLinkSpeedMult = 3;
	private void Start () {
		initialSpeed = navMeshAgent.speed;
		navMeshAgent.isStopped = true;
		// navMeshAgent.updateRotation = false;
		animator.SetTrigger("Spawn");
	}

	bool canJump = true;
	private void Update() {

		var forward = transform.InverseTransformDirection(navMeshAgent.desiredVelocity).z;
		var turn = Vector3.SignedAngle(transform.forward, navMeshAgent.desiredVelocity, Vector3.up);

		Debug.Log(string.Format("Forward {0}, Turn {1}", forward, turn));

		animator.SetFloat("Forward", forward, 0.1f, Time.deltaTime);
		animator.SetFloat("Turn", turn, 0.1f, Time.deltaTime);

		if (navMeshAgent.isOnOffMeshLink) navMeshAgent.speed = initialSpeed * offMeshLinkSpeedMult;
		else navMeshAgent.speed = initialSpeed;

		if (navMeshAgent.isOnOffMeshLink && canJump) {
			rootAnimator.SetTrigger("MeshTransitionJump");
			animator.SetTrigger("MeshTransitionJump");
		}
		canJump = !navMeshAgent.isOnOffMeshLink;

		if (!playerManager.currentInstance.playerEntity.alive) return;
		if (!attacking) {
			navMeshAgent.destination = playerManager.currentInstance.playerEntity.transform.position;
		} else {
			if ((Time.time - lastAttack > attackCooldown)) {
				audioManager.PlayRandom("Attack");
				animator.SetTrigger("Attack");
				lastAttack = Time.time;
			}
		}

	}

	public void Spawn() {
		audioManager.Play("SpawnCry");
	}
	public void StartTrack() {
		navMeshAgent.isStopped = false;
	}
	public void Attack() {
		if (playerManager.currentInstance.playerEntity.alive && attacking) {
			audioManager.Play("AttackHit");
			playerManager.currentInstance.playerEntity.alive = false;
			attacking = false;
		}
	}

	public void Walk() {
		audioManager.Play("FootStep");
	}
}
