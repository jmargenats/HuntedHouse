using UnityEngine;

public class PhotoInteractable : MonoBehaviour, IInteractable
{
    public PhotoViewerUI photoViewerUI;
    public Sprite frontSprite;
    public Sprite backSprite;

    public DialogueManager dialogueManager;
    [TextArea(2, 4)]
    public string missingPhotoMessage =
        "No puedo verla bien.";

    public void Interact()
    {
        if (
            photoViewerUI == null ||
            frontSprite == null
        )
        {
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(
                    missingPhotoMessage
                );
            }

            return;
        }

        photoViewerUI.OpenPhoto(
            frontSprite,
            backSprite
        );
    }
}
