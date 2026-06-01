using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class RadioPuzzle :
    MonoBehaviour,
    IInteractable
{
    public DialogueManager dialogueManager;

    public Inventario inventario;

    public GameObject subject01;

    [Header("Audio")]
    public AudioSource radioSource;
    public AudioClip doctorTape;
    public AudioSource voiceSource;
    public AudioClip playerWhatNoise;

    [Header("Player")]
    public FirstPersonController playerController;

    public PlayerInput playerInput;

    public void Interact()
    {
        // Primera vez que examina la radio
        if (!GameManager.Instance.radioDiscovered)
        {
            GameManager.Instance.radioDiscovered = true;

            if (GameManager.Instance.cassetteDiscovered)
            {
                dialogueManager.ShowDialogue(
                    "Una radio antigua.\nMe pregunto si la cinta que vi podría funcionar acá."
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
                "Esa cinta que vi antes podría funcionar aquí."
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
                "Quizás debería probar con esa cinta."
            );

            return;
        }

        PlayCassette();
    }

    void PlayCassette()
    {
        GameManager.Instance.radioPlayed = true;

        GameManager.Instance.collectedItems
            .Remove("Cassette");

        dialogueManager.ShowDialogue(
            "Insertás el cassette en la radio..."
        );

        if (playerInput != null)
        {
            playerInput.enabled = false;
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

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

        // Aparece el monstruo
        SpawnEnemy();

        // Devolver control al jugador
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }

        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    void SpawnEnemy()
    {
        if (subject01 != null)
        {
            subject01.SetActive(true);
        }

        dialogueManager.ShowDialogue(
            "¿Qué fue ese ruido...?"
        );
    }
}