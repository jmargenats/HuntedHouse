using UnityEngine;

public class PenPuzzle :
    MonoBehaviour,
    IInteractable,
    IPickupable
{
    public DialogueManager dialogueManager;

    public Inventario inventario;

    public Sprite icon;

    public string itemType = "Lapicera";

    public void Interact()
    {
        if (!GameManager.Instance.cassetteNeedTool)
        {
            dialogueManager.ShowDialogue(
                "Un par de lapiceras. Dudo que todavía funcionen."
            );

            return;
        }

        dialogueManager.ShowDialogue(
            "Podría servirme para destrabar esa cinta."
        );
    }

    public bool CanPickup()
    {
        return GameManager.Instance.cassetteNeedTool;
    }

    public void Pickup()
    {
        inventario.AddItemToInventory(
            icon,
            itemType
        );

        GameManager.Instance.penCollected =
            true;

        gameObject.SetActive(false);
    }
}