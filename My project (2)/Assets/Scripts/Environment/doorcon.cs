using UnityEngine;

public class doorcon : MonoBehaviour, IExaminable
{
    public AudioSource doorSound;
    public AudioClip doorOpen;
    public string doorstatus;

    public DialogueManager dialogueManager;

    [TextArea(2, 5)]
    public string lockedText = "La puerta está cerrada.";
    public string unlockedText = "La puerta está abierta.";

    public void Examine()
    {
        mapcon.doorStates[gameObject.name] = doorstatus;

        if (dialogueManager == null)
        {
            Debug.LogWarning("Falta DialogueManager en " + gameObject.name);
            return;
        }

        if (doorstatus == "locked")
        {
            dialogueManager.ShowDialogue(lockedText);
        }
        else if (doorstatus == "unlock")
        {
            doorSound.clip = doorOpen;

            doorSound.Play();
            dialogueManager.ShowDialogue(unlockedText);
        }
    }
}