using UnityEngine;

public class CassettePuzzle :
    MonoBehaviour,
    IInteractable,
    IPickupable
{
    public DialogueManager dialogueManager;

    public Inventario inventario;

    public Sprite icon;

    public string itemType = "Cassette";

    public void Interact()
    {
        // Primera vez
        if (!GameManager.Instance.radioDiscovered)
        {
            dialogueManager.ShowDialogue(
                "Una cinta vieja. Parece dañada."
            );

            return;
        }

        // Descubre los agujeros
        if (!GameManager.Instance.cassetteUnlocked)
        {
            string selectedItem =
                inventario.DevolverItem();

            // No tiene lapicera seleccionada
            if (selectedItem != "Lapicera")
            {
                dialogueManager.ShowDialogue(
                    "Mmmh... Parece tener dos orificios. Quizás pueda destrabarla con algo."
                );

                GameManager.Instance.cassetteNeedTool =
                    true;

                return;
            }

            // Tiene lapicera seleccionada
            dialogueManager.ShowDialogue(
                "Usás la lapicera para girar la cinta.\nLa punta se rompe."
            );

            inventario.RemoveItem(
                "Lapicera"
            );

            GameManager.Instance.penCollected =
                false;

            GameManager.Instance.cassetteUnlocked =
                true;

            return;
        }

        dialogueManager.ShowDialogue(
            "La cinta ya está destrabada."
        );
    }

    public bool CanPickup()
    {
        return
            GameManager.Instance.cassetteUnlocked
            &&
            !GameManager.Instance.cassetteCollected;
    }

    public void Pickup()
    {
        inventario.AddItemToInventory(
            icon,
            itemType
        );

        GameManager.Instance.cassetteCollected =
            true;

        dialogueManager.ShowDialogue(
            "Cassette obtenido."
        );

        gameObject.SetActive(false);
    }
}