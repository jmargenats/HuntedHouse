using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    public Inventario inventario;

    public Sprite iconoInventario;

    public string itemType;

    public DialogueManager dialogueManager;
    [TextArea(2, 4)]
    public string[] pickupDialogues;
    public void Recolectar()
    {
        inventario.AddItemToInventory(iconoInventario, itemType);

        if (dialogueManager != null && pickupDialogues.Length > 0)
        {
            string randomDialogue = pickupDialogues[Random.Range(0, pickupDialogues.Length)];
            dialogueManager.ShowDialogue(randomDialogue);
        }

        gameObject.SetActive(false);
    }
}
