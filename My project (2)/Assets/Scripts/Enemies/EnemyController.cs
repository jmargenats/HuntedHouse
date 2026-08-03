using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;

    [Header("NavMesh")]
    [Tooltip("Distancia maxima para corregir un agente que quedo apenas fuera del NavMesh.")]
    [Min(0.1f)] public float navMeshSearchRadius = 1.5f;

    [Tooltip("Evita que el agente salte al NavMesh de otro piso o al de un mueble.")]
    [Min(0.05f)] public float maxVerticalCorrection = 0.75f;

    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 startPosition;

    private bool chasing = false;
    private bool returningHome = false;
    private bool reportedMissingNavMesh = false;

    [Header("Audio")]
    public AudioSource monsterAudio;

    public AudioClip monsterGrowl;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator =
            GetComponentInChildren<Animator>();

        TryPlaceOnNavMesh();

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
            agent == null ||
            !agent.enabled
        )
        {
            return;
        }

        if (!agent.isOnNavMesh)
        {
            TryPlaceOnNavMesh();

            if (!agent.isOnNavMesh)
            {
                SetMovingAnimation(false);
                return;
            }
        }

        if (
            chasing &&
            player != null
        )
        {
            agent.SetDestination(
                player.position
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
                    "Volvio a casa"
                );

                returningHome = false;

                agent.ResetPath();
            }
        }

        bool moving =
            agent.velocity.magnitude > 0.1f;

        SetMovingAnimation(moving);

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

    private bool TryPlaceOnNavMesh()
    {
        if (
            agent == null ||
            !agent.enabled
        )
        {
            return false;
        }

        if (agent.isOnNavMesh)
        {
            return true;
        }

        NavMeshHit hit;

        if (
            !NavMesh.SamplePosition(
                transform.position,
                out hit,
                navMeshSearchRadius,
                agent.areaMask
            ) ||
            Mathf.Abs(hit.position.y - transform.position.y) >
                maxVerticalCorrection
        )
        {
            if (!reportedMissingNavMesh)
            {
                Debug.LogError(
                    gameObject.name +
                    " no tiene un NavMesh valido en este mismo piso.",
                    this
                );

                reportedMissingNavMesh = true;
            }

            return false;
        }

        bool placed = agent.Warp(hit.position);

        if (placed)
        {
            reportedMissingNavMesh = false;
        }

        if (!placed)
        {
            Debug.LogError(
                "No se pudo colocar " +
                gameObject.name +
                " sobre el NavMesh.",
                this
            );
        }

        return placed;
    }

    private void SetMovingAnimation(bool moving)
    {
        if (animator != null && animator.enabled)
        {
            animator.SetBool(
                "isMoving",
                moving
            );
        }
    }
}
