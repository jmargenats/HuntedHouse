using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public string disabledIfEnemyDefeatedID;

    [TextArea(2, 4)]
    public string[] dialogueOptions;

    public bool onlyOnce = true;
    private bool alreadyTriggered = false;

    void Start()
    {
        if (!string.IsNullOrEmpty(disabledIfEnemyDefeatedID) &&
            GameManager.Instance != null &&
            GameManager.Instance.IsEnemyDefeated(disabledIfEnemyDefeatedID))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (onlyOnce && alreadyTriggered) return;

        if (other.CompareTag("Player"))
        {
            alreadyTriggered = true;

            if (dialogueManager != null && dialogueOptions.Length > 0)
            {
                string randomDialogue = dialogueOptions[
                    Random.Range(0, dialogueOptions.Length)
                ];

                dialogueManager.ShowDialogue(randomDialogue);
            }
        }
    }
}