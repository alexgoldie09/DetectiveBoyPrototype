using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim; // Assign the NPC's Animator component
    [SerializeField] private NavMeshAgent agent; // Reference to NPC's navmesh

    [Header("Patrol variables")]
    [SerializeField] private Transform player; // Assign the player's transform in the inspector
    [SerializeField] private Transform[] patrolPoints; // Assign the patrol points in the inspector
    [SerializeField] private float interactionDistance = 5f; // Distance within which the NPC stops and looks at the player
    [SerializeField] private float waitTime = 2f; // Time to wait at each point

    private int lastPatrolPointIndex = -1; // Store last patrol point index
    private bool isWaiting = false; // Reference to if the NPC is waiting
    private bool playerInRange = false; // Reference to if the player is in range

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (patrolPoints.Length > 0)
        {
            MoveToRandomPatrolPoint(); // Start by moving to a random point
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (player == null) return; // Ensure player is not null

        float distanceToPlayer = CheckDistanceToPlayer();
        if (distanceToPlayer < interactionDistance)
        {
            StopAndLookAtPlayer();
            playerInRange = true;
        }
        else if (playerInRange)
        {
            ResumePatrol();
            playerInRange = false;
        }
        else if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(WaitAtPoint()); // Wait at the current point
        }

        UpdateAnimation();

        //// Update animator based on NPC movement
        //if (agent.velocity.sqrMagnitude > 0.1f && !agent.isStopped) // NPC is moving
        //{
        //    anim.SetBool("isWalking", true);
        //}
        //else // NPC is idle
        //{
        //    anim.SetBool("isWalking", false);
        //}
    }

    // Function to stop NPC if player is in range
    private void StopAndLookAtPlayer()
    {
        // Stop the NPC from moving
        agent.isStopped = true;
        anim.SetBool("isWalking", false); // Switch to idle animation

        // Turn to face the player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Keep the NPC level when turning

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Function to continue patrol if player leaves range
    private void ResumePatrol()
    {
        if (patrolPoints.Length > 0)
        {
            // Resume movement along patrol points
            agent.isStopped = false;
            MoveToRandomPatrolPoint(); // Move to a new random patrol point
            anim.SetBool("isWalking", true); // Switch to walk animation
        }
    }

    // Function to wait at point once NPC reaches
    private IEnumerator WaitAtPoint()
    {
        isWaiting = true;

        // NPC waits, switch to idle animation
        anim.SetBool("isWalking", false);

        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        MoveToRandomPatrolPoint(); // Choose a new random point after waiting

        isWaiting = false;
    }

    private void MoveToRandomPatrolPoint()
    {
        if (patrolPoints.Length > 0)
        {
            int randomIndex;

            // Ensure the new index is different from the last
            do
            {
                randomIndex = Random.Range(0, patrolPoints.Length);
            } while (randomIndex == lastPatrolPointIndex);

            lastPatrolPointIndex = randomIndex; // Update last patrol point index
            agent.SetDestination(patrolPoints[randomIndex].position); // Move to the random patrol point
        }
    }

    private void UpdateAnimation()
    {
        if (agent.velocity.sqrMagnitude > 0.1f && !agent.isStopped)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    // Function to check distance between interactable and player
    private float CheckDistanceToPlayer() => Vector3.Distance(transform.position, player.position);
}
