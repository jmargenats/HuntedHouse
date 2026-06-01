using System.Collections;
using UnityEngine;

public class NewspaperInteractable : MonoBehaviour, IInteractable
{
    public GameObject newspaperImageUI;
    public DialogueManager dialogueManager;
    public GameObject recordPrompt;

    [TextArea(2, 4)]
    public string afterReadDialogue =
        "Esto es lo que vine a buscar.\nSi voy a entrar ahí, mejor que quede todo grabado.";

    private bool isReading = false;
    private bool alreadyRead = false;

    public void Interact()
    {
        isReading = !isReading;

        newspaperImageUI.SetActive(isReading);

        if (!isReading && !alreadyRead)
        {
            alreadyRead = true;

            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(afterReadDialogue);
            }

            if (recordPrompt != null)
            {
                StartCoroutine(ShowRecordPrompt());
            }
        }
    }

    IEnumerator ShowRecordPrompt()
    {
        // Espera a que termine el diálogo
        yield return new WaitForSeconds(
            dialogueManager != null
                ? dialogueManager.displayTime
                : 3f
        );

        recordPrompt.SetActive(true);

        yield return new WaitForSeconds(4f);

        recordPrompt.SetActive(false);
    }
}