using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 startPosition;
    private bool chasing = false;
    private bool returningHome = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (chasing && player != null)
        {
            agent.SetDestination(player.position);
        }
        else if (returningHome)
        {
            agent.SetDestination(startPosition);

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                returningHome = false;
                agent.ResetPath();
            }
        }

        bool moving = agent.velocity.magnitude > 0.1f;

        if (animator != null)
        {
            animator.SetBool("isMoving", moving);
        }
    }

    public void StartChasing()
    {
        chasing = true;
        returningHome = false;
    }
    public void StopChasingAndReturn()
    {
        chasing = false;
        returningHome = true;
    }
}