using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [TextArea(2, 4)]
    public string dialogueText;

    public bool onlyOnce = true;
    private bool alreadyTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (onlyOnce && alreadyTriggered) return;

        if (other.CompareTag("Player"))
        {
            alreadyTriggered = true;
            dialogueManager.ShowDialogue(dialogueText);
        }
    }
}