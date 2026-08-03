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

    public bool HasBeenRead => alreadyRead;

    private PlayerInteractions playerInteractions;

    void Start()
    {
        playerInteractions =
            FindObjectOfType<PlayerInteractions>();
    }

    public void Interact()
    {
        if (isReading)
            return;

        isReading = true;

        newspaperImageUI.SetActive(true);

        if (playerInteractions != null)
        {
            playerInteractions.enabled = false;
        }
    }

    void Update()
    {
        if (
            isReading &&
            Input.GetKeyDown(KeyCode.F)
        )
        {
            CloseNewspaper();
        }
    }

    void CloseNewspaper()
    {
        isReading = false;

        newspaperImageUI.SetActive(false);

        if (playerInteractions != null)
        {
            playerInteractions.enabled = true;
        }

        if (!alreadyRead)
        {
            alreadyRead = true;

            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(
                    afterReadDialogue
                );
            }

            if (recordPrompt != null)
            {
                StartCoroutine(
                    ShowRecordPrompt()
                );
            }
        }
    }

    IEnumerator ShowRecordPrompt()
    {
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