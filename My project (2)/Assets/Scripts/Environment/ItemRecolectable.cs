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

    public DialogueManager dialogueManager;
    [TextArea(2, 4)]
    public string[] pickupDialogues;
    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.HasItem(itemType))
        {
            gameObject.SetActive(false);
        }
    }
    public void Recolectar()
    {
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
