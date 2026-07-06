using UnityEngine;

public class TomasDialogue : MonoBehaviour, IInteractable
{
    public DialogueManager dialogueManager;

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

    public void Interact()
    {
        if (dialogueManager == null)
            return;

        int random = Random.Range(0, dialogues.Length);

        dialogueManager.ShowDialogue(dialogues[random]);
    }
}