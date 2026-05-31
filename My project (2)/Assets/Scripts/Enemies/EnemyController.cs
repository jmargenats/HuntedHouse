using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        agent.SetDestination(player.position);

        bool moving = agent.hasPath && agent.remainingDistance > agent.stoppingDistance;

        Debug.Log("Remaining: " + agent.remainingDistance);

// animator.SetBool("isMoving", moving);
//Preguntarle a luz
    }
}