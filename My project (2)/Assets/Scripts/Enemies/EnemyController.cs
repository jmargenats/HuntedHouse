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

    [Header("Audio")]
    public AudioSource monsterAudio;

    public AudioClip monsterGrowl;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator =
            GetComponentInChildren<Animator>();

        startPosition =
            transform.position;

        Debug.Log(
            gameObject.name +
            " iniciado."
        );

        if (
            monsterAudio != null &&
            monsterGrowl != null
        )
        {
            monsterAudio.clip =
                monsterGrowl;

            monsterAudio.loop = true;

            monsterAudio.playOnAwake =
                false;
        }
    }

    void Update()
    {
        if (
            chasing &&
            player != null
        )
        {
            agent.SetDestination(
                player.position
            );

            Debug.Log(
                "Persiguiendo jugador"
            );
        }
        else if (returningHome)
        {
            agent.SetDestination(
                startPosition
            );

            if (
                !agent.pathPending &&
                agent.remainingDistance <=
                agent.stoppingDistance
            )
            {
                Debug.Log(
                    "Volvió a casa"
                );

                returningHome = false;

                agent.ResetPath();
            }
        }

        bool moving =
            agent.velocity.magnitude > 0.1f;

        Debug.Log(
            "Velocidad: " +
            agent.velocity.magnitude
        );

        if (animator != null)
        {
            animator.SetBool(
                "isMoving",
                moving
            );
        }

        if (monsterAudio != null)
        {
            if (
                chasing &&
                moving
            )
            {
                if (!monsterAudio.isPlaying)
                {
                    monsterAudio.Play();
                }
            }
            else
            {
                if (monsterAudio.isPlaying)
                {
                    monsterAudio.Stop();
                }
            }
        }
    }

    public void StartChasing()
    {
        Debug.Log(
            "START CHASING"
        );

        chasing = true;

        returningHome = false;
    }

    public void StopChasingAndReturn()
    {
        Debug.Log(
            "STOP CHASING"
        );

        chasing = false;

        returningHome = true;
    }
}