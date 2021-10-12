using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;
    public ThirdPersonCharacter character;
    private Animator animator;
    private PlayerController player;
    private AudioRig audioManager;
    private float lastAttack;
    [SerializeField] private float attackCooldown = 2;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //not the good way to do it i need to see with guillaume about that
        player = movePositionTransform.GetComponent<PlayerController>();
        audioManager = GetComponent<AudioRig>();
    }

    private void Start () {
        navMeshAgent.isStopped = true;
        navMeshAgent.updateRotation = false;
        animator.SetTrigger("Spawn");
    }

    private void Update() {
        navMeshAgent.destination = movePositionTransform.position;
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            character.Move(navMeshAgent.desiredVelocity, false, false);
        } else
        {
            character.Move(Vector3.zero, false, false);
            if (player.alive && (Time.time - lastAttack > attackCooldown)) {
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
        if( navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
            audioManager.Play("AttackHit");
            player.alive = false;
        }
    }
    
    public void Walk() {
        audioManager.Play("FootStep");
    }
}
