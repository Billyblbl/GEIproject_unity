using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class MonsterAI : MonoBehaviour
{

	public GlobalInterface<PlayerManager> playerManager;
    // [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;
    public ThirdPersonCharacter character;
    private Animator animator;
    // private PlayerController player;
    private AudioRig audioManager;
    private float lastAttack;
    [SerializeField] private float attackCooldown = 2;

	bool attacking = false;

	public void OnPlayerInRange() {
		Debug.Log("In range");
        character.Move(Vector3.zero, false, false);
		attacking = true;
		navMeshAgent.destination = transform.position;
	}

	public void OnPlayerOutOfRange() {
		Debug.Log("Out of range");
		attacking = false;
	}

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioManager = GetComponent<AudioRig>();
    }


    private void Start () {
        navMeshAgent.isStopped = true;
        navMeshAgent.updateRotation = false;
        animator.SetTrigger("Spawn");
    }

    private void Update() {
		if (!playerManager.currentInstance.playerEntity.alive) return;
		if (!attacking) {
	        navMeshAgent.destination = playerManager.currentInstance.playerEntity.transform.position;
			character.Move(navMeshAgent.desiredVelocity, false, false);
		} else if ((Time.time - lastAttack > attackCooldown)) {
            audioManager.PlayRandom("Attack");
            animator.SetTrigger("Attack");
            lastAttack = Time.time;
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
