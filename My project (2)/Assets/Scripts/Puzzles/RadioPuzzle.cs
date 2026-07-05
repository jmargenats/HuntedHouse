using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using StarterAssets;

public class RadioPuzzle :
    MonoBehaviour,
    IInteractable
{
    public DialogueManager dialogueManager;
    public GameObject screwdriverCross;
    public Inventario inventario;
    public doorcon atticDoorExamine;
    public GameObject subject01;

    [Header("Audio")]
    public AudioSource radioSource;
    public AudioClip radioStatic;
    public AudioClip doctorTape;
    public AudioSource voiceSource;
    public AudioClip playerWhatNoise;
    public AudioClip fallNoise;

    [Header("Attic Fall")]
    public Door atticDoor;
    public Transform atticDoorObject;
    public Vector3 atticDoorOpenEulerOffset = new Vector3(-90f, 0f, 0f);
    public float atticDoorOpenDuration = 0.5f;
    public Transform enemyFallStart;
    public Transform enemyFallEnd;
    public float doorOpenDelay = 0.35f;
    public float fallDuration = 0.9f;
    public float fallbackFallDistance = 6f;
    public float chaseDelayAfterFall = 0.4f;
    public bool startChasingAfterFall = true;

    [Header("Player")]
    public FirstPersonController playerController;

    public PlayerInput playerInput;

    void Start()
    {
        if (screwdriverCross != null)
        {
            screwdriverCross.SetActive(GameManager.Instance.radioPlayed);
        }
        if (!GameManager.Instance.radioPlayed)
            return;

        if (atticDoor != null)
        {
            atticDoor.isOpen = true;
        }

        if (atticDoorObject != null)
        {
            atticDoorObject.rotation =
                atticDoorObject.rotation *
                Quaternion.Euler(atticDoorOpenEulerOffset);
        }

        if (atticDoorExamine != null)
        {
            atticDoorExamine.doorstatus = "unlock";
        }

        if (subject01 != null)
        {
            subject01.SetActive(true);
        }

    }
    public void Interact()
    {
        radioSource.clip = radioStatic;

        radioSource.Play();
        // Primera vez que examina la radio
        if (!GameManager.Instance.radioDiscovered)
        {
            GameManager.Instance.radioDiscovered = true;

            if (GameManager.Instance.cassetteDiscovered)
            {
                dialogueManager.ShowDialogue(
                    "Me pregunto si la cinta que vi podría funcionar acá."
                );
            }
            else
            {
                dialogueManager.ShowDialogue(
                    "Una radio antigua."
                );
            }
 
            return;
        }

        // Ya vio la cinta y vuelve a la radio
        if (
            GameManager.Instance.cassetteDiscovered &&
            !GameManager.Instance.cassetteNeedTool
        )
        {
            GameManager.Instance.cassetteNeedTool = true;

            dialogueManager.ShowDialogue(
                "Esa cinta que vi antes podría funcionar si la arreglo."
            );

            return;
        }

        // Todavía no consiguió el cassette
        if (!GameManager.Instance.cassetteCollected)
        {
            dialogueManager.ShowDialogue(
                "Necesito una cinta para probarla."
            );

            return;
        }

        // Ya reprodujo la grabación
        if (GameManager.Instance.radioPlayed)
        {
            return;
        }

        string selectedItem =
            inventario.DevolverItem();

        // Tiene cassette pero no lo tiene equipado
        if (selectedItem != "Cassette")
        {
            dialogueManager.ShowDialogue(
                "Quizás debería probar con la cinta."
            );

            return;
        }

        PlayCassette();
    }

    void PlayCassette()
    {
        GameManager.Instance.radioPlayed = true;

        //GameManager.Instance.collectedItems
        //    .Remove("Cassette");

        inventario.RemoveItem("Cassette");

        dialogueManager.ShowDialogue(
            "Insertás el cassette en la radio..."
        );
        /*
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }
        */

        StartCoroutine(
            PlayTapeSequence()
        );
    }

    IEnumerator PlayTapeSequence()
    {
        // Pequeña pausa después de insertar el cassette
        yield return new WaitForSeconds(2f);

        // Grabación del médico
        if (
            radioSource != null &&
            doctorTape != null
        )
        {
            radioSource.clip = doctorTape;

            radioSource.Play();

            yield return new WaitForSeconds(
                doctorTape.length
            );
        }

        // Silencio incómodo
        yield return new WaitForSeconds(1f);

        // player habla
        /*
        if (
            voiceSource != null &&
            playerWhatNoise != null
        )
        {
            voiceSource.PlayOneShot(
                playerWhatNoise
            );

            yield return new WaitForSeconds(
                playerWhatNoise.length
            );
        }
        yield return new WaitForSeconds(1f);*/
        if (
            voiceSource != null &&
            fallNoise != null
        )
        {
            voiceSource.PlayOneShot(fallNoise);
        }

        yield return StartCoroutine(
            DropEnemyFromAttic()
        );
        /*
        // Devolver control al jugador
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }

        if (playerController != null)
        {
            playerController.enabled = true;
        }*/
    }

    IEnumerator OpenAtticDoor()
    {
        if (atticDoor != null)
        {
            atticDoor.isOpen = true;
        }

        if (atticDoorObject == null)
        {
            yield break;
        }

        Quaternion closedRotation =
            atticDoorObject.rotation;

        Quaternion openRotation =
            closedRotation * Quaternion.Euler(
                atticDoorOpenEulerOffset
            );

        float elapsed = 0f;

        while (elapsed < atticDoorOpenDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / Mathf.Max(atticDoorOpenDuration, 0.01f)
                );

            atticDoorObject.rotation =
                Quaternion.Slerp(
                    closedRotation,
                    openRotation,
                    t
                );

            yield return null;
        }

        atticDoorObject.rotation =
            openRotation;
        if (atticDoorExamine != null)
        {
            atticDoorExamine.doorstatus = "unlock";
        }
    }
    IEnumerator DropEnemyFromAttic()
    {
        yield return StartCoroutine(
            OpenAtticDoor()
        );

        yield return new WaitForSeconds(
            doorOpenDelay
        );

        if (subject01 == null)
        {
            yield break;
        }

        EnemyController enemyController =
            subject01.GetComponent<EnemyController>();

        EnemyCombatTrigger combatTrigger =
            subject01.GetComponent<EnemyCombatTrigger>();

        NavMeshAgent agent =
            subject01.GetComponent<NavMeshAgent>();

        if (enemyController != null)
        {
            enemyController.enabled = false;
        }

        if (combatTrigger != null)
        {
            combatTrigger.enabled = false;
        }

        if (agent != null)
        {
            agent.enabled = false;
        }

        Vector3 startPosition =
            enemyFallStart != null
            ? enemyFallStart.position
            : subject01.transform.position;

        Vector3 endPosition =
            GetEnemyFallEndPosition(
                startPosition
            );

        //if (
        //    NavMesh.SamplePosition(
        //        endPosition,
        //        out NavMeshHit navHit,
        //        2f,
        //        NavMesh.AllAreas
        //    )
        //)
        //{
        //    endPosition = navHit.position;
        //}

        subject01.transform.position =
            startPosition;

        subject01.SetActive(true);

        float elapsed = 0f;

        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / Mathf.Max(fallDuration, 0.01f)
                );

            t = t * t * (3f - 2f * t);

            subject01.transform.position =
                Vector3.Lerp(
                    startPosition,
                    endPosition,
                    t
                );

            yield return null;
        }

        subject01.transform.position =
            endPosition;

        if (agent != null)
        {
            agent.enabled = true;
            agent.Warp(endPosition);
        }

        if (combatTrigger != null)
        {
            combatTrigger.enabled = true;
        }

        yield return new WaitForSeconds(
            chaseDelayAfterFall
        );

        if (enemyController != null)
        {
            enemyController.enabled = true;

            if (startChasingAfterFall)
            {
                enemyController.StartChasing();
            }
        }

        dialogueManager.ShowDialogue(
            "¿Qué fue ese ruido...?"
        );
        subject01.transform.position = endPosition;

        if (screwdriverCross != null)
        {
            screwdriverCross.SetActive(true);
        }
    }

    Vector3 GetEnemyFallEndPosition(
        Vector3 startPosition
    )
    {
        if (enemyFallEnd != null)
        {
            return enemyFallEnd.position;
        }

        if (
            Physics.Raycast(
                startPosition,
                Vector3.down,
                out RaycastHit hit,
                fallbackFallDistance + 2f
            )
        )
        {
            return hit.point;
        }

        return
            startPosition +
            Vector3.down * fallbackFallDistance;
    }
}