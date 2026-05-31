using UnityEngine;

public class RadioPuzzle :
    MonoBehaviour,
    IInteractable
{
    public DialogueManager dialogueManager;

    public Inventario inventario;

    public GameObject subject01;

    public void Interact()
    {
        // Primera vez
        if (!GameManager.Instance.radioDiscovered)
        {
            dialogueManager.ShowDialogue(
                "Una radio antigua.\nMe pregunto si la cinta atascada que vi podría funcionar acá."
            );

            GameManager.Instance.radioDiscovered =
                true;

            return;
        }

        // Todavía no tiene cassette
        if (!GameManager.Instance.cassetteCollected)
        {
            dialogueManager.ShowDialogue(
                "Necesito una cinta para probarla."
            );

            return;
        }

        // Ya reprodujo
        if (GameManager.Instance.radioPlayed)
        {
            return;
        }

        string selectedItem =
            inventario.DevolverItem();

        // Tiene el cassette pero no lo tiene equipado
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
        GameManager.Instance.radioPlayed =
            true;

        inventario.RemoveItem(
            "Cassette"
        );

        GameManager.Instance.cassetteCollected =
            false;

        dialogueManager.ShowDialogue(
            "Insertás el cassette en la radio.\nLa grabación comienza a reproducirse..."
        );

        SpawnEnemy();
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