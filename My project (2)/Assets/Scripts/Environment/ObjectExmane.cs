using UnityEngine;

public class ExaminableObject :
    MonoBehaviour,
    IExaminable
{
    public DialogueManager dialogueManager;

    [TextArea(2, 5)]
    public string[] examineTexts;

    public void Examine()
    {
        if (
            dialogueManager == null ||
            examineTexts.Length == 0
        )
            return;

        string text =
            examineTexts[
                Random.Range(
                    0,
                    examineTexts.Length
                )
            ];

        dialogueManager.ShowDialogue(
            text
        );
    }
}