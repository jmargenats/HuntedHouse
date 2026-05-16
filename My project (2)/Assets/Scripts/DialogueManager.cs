using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public float displayTime = 3f;

    private Coroutine currentDialogue;

    public void ShowDialogue(string message)
    {
        if (currentDialogue != null)
        {
            StopCoroutine(currentDialogue);
        }

        currentDialogue = StartCoroutine(ShowDialogueRoutine(message));
    }

    IEnumerator ShowDialogueRoutine(string message)
    {
        dialogueText.gameObject.SetActive(true);
        dialogueText.text = message;

        yield return new WaitForSeconds(displayTime);

        dialogueText.gameObject.SetActive(false);
    }
}