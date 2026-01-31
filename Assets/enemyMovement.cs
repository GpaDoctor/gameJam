using UnityEngine;
using UnityEngine.AI;  // Needed for NavMeshAgent

public class EnemyMovement : MonoBehaviour
{
    public Transform player;      // Assign in Inspector, or find by tag
    public NavMeshAgent agent;
    public Animator animator;
    public BoxCollider boxCollider;
    public CapsuleCollider capsuleCollider;
    public bool playerInBoxCollider = false;
    public bool playerInCapsuleCollider = false;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;  // Array of patrol points
    public float patrolWaitTime = 2f; // Time to wait at each patrol point
    private int currentPatrolIndex = 0;
    private float patrolWaitTimer = 0f;

    void Start()
    {
        // Optional: auto-find player by tag if not assigned manually
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        // Check if player is within box collider bounds
        if (boxCollider != null && boxCollider.isTrigger)
        {
            if (boxCollider.bounds.Contains(player.position))
            {
                playerInBoxCollider = true;
            }
            else
            {
                playerInBoxCollider = false;
            }
        }

        // Check if player is within capsule collider bounds
        if (capsuleCollider != null && capsuleCollider.isTrigger)
        {
            if (capsuleCollider.bounds.Contains(player.position))
            {
                playerInCapsuleCollider = true;
            }
            else
            {
                playerInCapsuleCollider = false;
            }
        }

        // Determine chase state based on collider triggers
        bool shouldChase = playerInBoxCollider && !playerInCapsuleCollider;

        // Chase if conditions are met
        if (shouldChase)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isChasing", true);

            // Make the agent face the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else
        {
            // Not chasing, patrol
            animator.SetBool("isChasing", false);
            Patrol();
        }
    }

    private void Patrol()
    {
        // If no patrol points are set, just idle
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            agent.SetDestination(transform.position);
            return;
        }

        // Check if agent has reached the current patrol point
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            // If agent is stuck or no path exists, try to recalculate
            if (agent.hasPath && agent.velocity.sqrMagnitude == 0f)
            {
                patrolWaitTimer -= Time.deltaTime;

                // Wait at patrol point
                if (patrolWaitTimer > 0)
                {
                    return;
                }
            }

            // Move to next patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            
            // Set destination using NavMesh pathfinding
            if (agent.SetDestination(patrolPoints[currentPatrolIndex].position))
            {
                patrolWaitTimer = patrolWaitTime;
            }
            else
            {
                Debug.LogWarning("Failed to set destination to patrol point: " + patrolPoints[currentPatrolIndex].name);
            }
        }
    }

    // Called when player enters a trigger collider on the enemy
    private void OnTriggerEnter(Collider other)
    {
        // No longer needed - bounds checking is done in Update()
    }

    // Called when player exits a trigger collider on the enemy
    private void OnTriggerExit(Collider other)
    {
        // No longer needed - bounds checking is done in Update()
    }
}
