using UnityEngine;

public class TomasDialogue : MonoBehaviour, IInteractable
{
    public DialogueManager dialogueManager;
    public SofiaPuzzle sofiaPuzzle;

    [TextArea]
    public string[] dialogues =
    {
        "No... no hagas ruido... está durmiendo.",

        "Prometí que nadie iba a entrar.",

        "Todavía está ahí... ¿verdad?",

        "No la despiertes... por favor.",

        "No podés pasar.",

        "Papá dijo que la cuidara.",

        "No... no abras esa puerta.",

        "Tengo que quedarme acá.",

        "Ella tiene miedo cuando hay gente.",

        "No voy a dejar que la lastimen.",

        "No entiendo por qué no responde...",

        "Todavía respira... ¿no?",

        "Si me quedo acá... todo va a estar bien.",

        "No puedo irme.",

        "...ella me necesita."
    };

    [TextArea]
    public string[] solvedDialogues;

    public void Interact()
    {
        if (dialogueManager == null)
            return;

        string[] currentDialogues = dialogues;

        if (sofiaPuzzle != null && sofiaPuzzle.IsSolved && solvedDialogues.Length > 0)
            currentDialogues = solvedDialogues;

        if (currentDialogues.Length == 0)
            return;

        int random = Random.Range(0, currentDialogues.Length);

        dialogueManager.ShowDialogue(currentDialogues[random]);
    }
}
