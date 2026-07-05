using UnityEngine;

public class ClockPuzzle : MonoBehaviour, IInteractable
{
    public DialogueManager dialogueManager;

    public ClockUI clockUI;

    public GameObject rewardObject;

    private bool solved = false;

    public void Interact()
    {
        if (solved)
            return;

        dialogueManager.ShowDialogue(
            "Parece que las agujas pueden moverse."
        );

        clockUI.OpenPuzzle(this);
    }

    public void CheckTime(int hour, int minute)
    {
        Debug.Log($"Comprobando {hour}:{minute}");

        if (hour == 10 && minute == 45)
        {
            solved = true;

            rewardObject.SetActive(true);

            dialogueManager.ShowDialogue(
                "Escucho un mecanismo..."
            );
        }
        else
        {
            dialogueManager.ShowDialogue(
                "No parece ser el horario correcto."
            );
        }
    }
}