using UnityEngine;

public class TomasDialogue : MonoBehaviour, IInteractable
{
    public DialogueManager dialogueManager;
    public Inventario inventario;

    public void Interact()
    {
        if (dialogueManager == null)
            return;

        // ----------------------------
        // DAR EL OSO
        // ----------------------------

        if (
            GameManager.Instance.bearUnlocked &&
            !GameManager.Instance.bearDelivered &&
            inventario.DevolverItem() == "Osito"
        )
        {
            GameManager.Instance.bearDelivered = true;

            inventario.RemoveItem("Osito");

            GameManager.Instance.tomasConversationStage = 4;

            dialogueManager.ShowDialogue(
                "...\n\nLo encontraste...\n\nGracias..."
            );

            return;
        }

        // ----------------------------
        // SI YA ESTÁ BUSCANDO EL OSO
        // ----------------------------

        if (
            GameManager.Instance.bearUnlocked &&
            !GameManager.Instance.bearDelivered
        )
        {
            dialogueManager.ShowDialogue(
                "¿Encontraste su osito...?"
            );

            return;
        }

        // ----------------------------
        // CONVERSACIONES
        // ----------------------------

        switch (GameManager.Instance.tomasConversationStage)
        {
            case 0:

                dialogueManager.ShowDialogue(
                    "No... no hagas ruido...\nEstá durmiendo."
                );

                GameManager.Instance.tomasConversationStage++;

                break;

            case 1:

                dialogueManager.ShowDialogue(
                    "Prometí que nadie iba a entrar."
                );

                GameManager.Instance.tomasConversationStage++;

                break;

            case 2:

                dialogueManager.ShowDialogue(
                    "Ella siempre dormía abrazando algo..."
                );

                GameManager.Instance.tomasConversationStage++;

                break;

            case 3:

                dialogueManager.ShowDialogue(
                    "No encuentro su osito...\nSin él no puede dormir..."
                );

                GameManager.Instance.bearUnlocked = true;

                break;

            // ----------------------------
            // DESPUÉS DEL OSO
            // ----------------------------

            case 4:

                dialogueManager.ShowDialogue(
                    "A ella le gustaba abrazarlo cuando tenía miedo..."
                );

                GameManager.Instance.tomasConversationStage++;

                break;

            case 5:

                dialogueManager.ShowDialogue(
                    "Papá decía que iba a curarla..."
                );

                GameManager.Instance.tomasConversationStage++;

                break;

            case 6:

                dialogueManager.ShowDialogue(
                    "Ella lloraba...\nNo quería más inyecciones..."
                );

                GameManager.Instance.tomasConversationStage++;

                break;

            case 7:

                dialogueManager.ShowDialogue(
                    "Quiero que termine todo esto...\n\nNo puedo detenerlo...\n\nPero vos sí."
                );

                GameManager.Instance.helpedTomasEscape = true;

                GameManager.Instance.tomasConversationStage++;

                break;

            default:

                dialogueManager.ShowDialogue(
                    "Por favor...\nDetenelo."
                );

                break;
        }
    }
}