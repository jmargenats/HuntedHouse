using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    public Inventario inventario;

    public Sprite iconoInventario;

    public string itemType;
    [Header("Audio")]
    public AudioClip pickupSound;
    [Header("Unlock")]
    public bool requiresBearUnlock = false;
    public DialogueManager dialogueManager;
    [TextArea(2, 4)]
    public string[] pickupDialogues;
    void Start()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.HasItem(itemType))
            {
                gameObject.SetActive(false);
                return;
            }

            if (
                requiresBearUnlock &&
                !GameManager.Instance.bearUnlocked
            )
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void Recolectar()
    {
        if (
        requiresBearUnlock &&
        !GameManager.Instance.bearUnlocked
        )
        {
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(
                    "Un viejo oso de peluche... No parece tener utilidad."
                );
            }

            return;
        }
        inventario.AddItemToInventory(
            iconoInventario,
            itemType
        );

        if (GameManager.Instance != null)
        {
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    pickupSound,
                    Camera.main.transform.position
                );
            }
            GameManager.Instance.AddItem(
                itemType
            );
        }

        if (
            dialogueManager != null &&
            pickupDialogues.Length > 0
        )
        {
            string randomDialogue =
                pickupDialogues[
                    Random.Range(
                        0,
                        pickupDialogues.Length
                    )
                ];

            dialogueManager.ShowDialogue(
                randomDialogue
            );
        }

        gameObject.SetActive(false);
    }
}
