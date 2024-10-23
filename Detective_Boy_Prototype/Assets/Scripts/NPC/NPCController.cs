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
    [SerializeField] private int npcId; // Reference to the NPC suspect ID
    [SerializeField] private bool isSuspect = false; // Reference to if the NPC is a suspect

    [Header("Patrol variables")]
    [SerializeField] private Transform player; // Assign the player's transform in the inspector
    [SerializeField] private Transform[] patrolPoints; // Assign the patrol points in the inspector
    [SerializeField] private float interactionDistance = 5f; // Distance within which the NPC stops and looks at the player
    [SerializeField] private float waitTime = 2f; // Time to wait at each point

    private int currentPointIndex = 0; // Reference to the current patrol point
    private bool isWaiting = false; // Reference to if the NPC is waiting
    private bool playerInRange = false; // Reference to if the player is in range

    private void Awake()
    {
        if (npcId <= 0)
        {
            npcId = Random.Range(1, 20000);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (patrolPoints.Length > 0)
        {
            // Start patrolling to the first point
            agent.SetDestination(patrolPoints[currentPointIndex].position);
            anim.SetBool("isWalking", true); // Set the NPC to walking animation
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (CheckDistanceToPlayer() < interactionDistance)
        {
            StopAndLookAtPlayer(); // Stop and look at the player
            playerInRange = true;
        }
        else if (playerInRange)
        {
            ResumePatrol(); // Resume patrol if the player leaves the interaction distance
            playerInRange = false;
        }
        else if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (patrolPoints.Length > 0)
            {
                StartCoroutine(WaitAtPoint()); // Continue patrol as normal
            }
        }

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
            agent.SetDestination(patrolPoints[currentPointIndex].position);
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

        // Move to the next patrol point
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPointIndex].position);

        // NPC resumes walking animation
        anim.SetBool("isWalking", true);

        isWaiting = false;
    }

    // Function to check distance between interactable and player
    private float CheckDistanceToPlayer() => Vector3.Distance(transform.position, player.position);

    #region Getters and Setters
    public int NPCId { get { return npcId; } set { npcId = value; } }
    public bool IsSuspect => isSuspect;
    #endregion
}
