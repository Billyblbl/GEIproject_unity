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

	// bool attacking = false;

	public enum BehaviorState {
		IDLE,
		CHASING,
		ATTACKING,
		REMOVING_OBSTACLE
	};

	public BehaviorState currentBehavior = BehaviorState.IDLE;

	public void OnPlayerInRange() {
		// character.Move(Vector3.zero, false, false);
		currentBehavior = BehaviorState.ATTACKING;
		// attacking = true;
		navMeshAgent.destination = transform.position;
	}

	public void OnPlayerOutOfRange() {
		currentBehavior = BehaviorState.CHASING;
	}

	public void OnObstacleInRange(DoorController door) {
		currentBehavior = BehaviorState.REMOVING_OBSTACLE;
		navMeshAgent.destination = transform.position;
		door.MonsterStartRemoving();
	}

	public void OnObstacleOutOfRange(DoorController door) {
		currentBehavior = BehaviorState.CHASING;
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

	void UpdateMovement() {
		var forward = transform.InverseTransformDirection(navMeshAgent.desiredVelocity).z;
		var turn = Vector3.SignedAngle(transform.forward, navMeshAgent.desiredVelocity, Vector3.up);

		// Debug.Log(string.Format("Forward {0}, Turn {1}", forward, turn));

		animator.SetFloat("Forward", forward, 0.1f, Time.deltaTime);
		animator.SetFloat("Turn", turn, 0.1f, Time.deltaTime);

		if (navMeshAgent.isOnOffMeshLink) navMeshAgent.speed = initialSpeed * offMeshLinkSpeedMult;
		else navMeshAgent.speed = initialSpeed;

		if (navMeshAgent.isOnOffMeshLink && canJump) {
			rootAnimator.SetTrigger("MeshTransitionJump");
			animator.SetTrigger("MeshTransitionJump");
		}
		canJump = !navMeshAgent.isOnOffMeshLink;
	}

	void UpdateBehavior() {
		switch (currentBehavior) {
			case BehaviorState.ATTACKING: {
				if ((Time.time - lastAttack > attackCooldown)) {
					audioManager.PlayRandom("Attack");
					animator.SetTrigger("Attack");
					lastAttack = Time.time;
				}
				break;
			}
			case BehaviorState.CHASING: {
				navMeshAgent.destination = playerManager.currentInstance.playerEntity.transform.position;
				break;
			}
			case BehaviorState.REMOVING_OBSTACLE: break;
			case BehaviorState.IDLE: {
				Debug.Log("IDLE, should move to CHASING");
				if (
					playerManager &&
					playerManager.currentInstance &&
					playerManager.currentInstance.playerEntity &&
					playerManager.currentInstance.playerEntity.alive
				) currentBehavior = BehaviorState.CHASING;
				break;
			}
		}
	}

	private void Update() {
		UpdateMovement();
		UpdateBehavior();
	}

	public void Spawn() {
		audioManager.Play("SpawnCry");
	}
	public void StartTrack() {
		navMeshAgent.isStopped = false;
	}
	public void Attack() {
		if (playerManager.currentInstance.playerEntity.alive && currentBehavior == BehaviorState.ATTACKING) {
			audioManager.Play("AttackHit");
			playerManager.currentInstance.playerEntity.alive = false;
			currentBehavior = BehaviorState.IDLE;
		}
	}

	public void Walk() {
		audioManager.Play("FootStep");
	}
}
