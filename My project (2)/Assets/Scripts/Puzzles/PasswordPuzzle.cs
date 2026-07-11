using System.Collections;
using UnityEngine;

public class PasswordPuzzle : MonoBehaviour, IInteractable
{
    public DialogueManager dialogueManager;
    public PasswordUI passwordUI;

    [Header("Solved")]
    public GameObject objectToHide;

    public Transform leftDoor;
    public Transform rightDoor;

    public Vector3 leftDoorOpenRotation = new Vector3(0, 90, 0);
    public Vector3 rightDoorOpenRotation = new Vector3(0, -90, 0);

    public float openDuration = 0.8f;

    private bool solved = false;

    public void Interact()
    {
        if (solved)
            return;

        if (passwordUI != null)
        {
            passwordUI.OpenPuzzle(this);
        }
    }

    public void CheckPassword(string value)
    {
        if (value.ToLower() == "elena")
        {
            solved = true;

            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(
                    "La contrasena es correcta."
                );
            }

            HideSolvedObject();

            StartCoroutine(
                OpenWardrobeDoors()
            );
        }
        else
        {
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(
                    "No parece ser la contrasena."
                );
            }
        }
    }

    void HideSolvedObject()
    {
        if (objectToHide == null)
            return;

        Renderer[] renderers =
            objectToHide.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        Collider[] colliders =
            objectToHide.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
    IEnumerator OpenWardrobeDoors()
    {
        Quaternion leftStart =
            leftDoor != null
            ? leftDoor.localRotation
            : Quaternion.identity;

        Quaternion rightStart =
            rightDoor != null
            ? rightDoor.localRotation
            : Quaternion.identity;

        Quaternion leftEnd =
            Quaternion.Euler(leftDoorOpenRotation);

        Quaternion rightEnd =
            Quaternion.Euler(rightDoorOpenRotation);

        float elapsed = 0f;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / Mathf.Max(openDuration, 0.01f)
                );

            if (leftDoor != null)
            {
                leftDoor.localRotation =
                    Quaternion.Slerp(leftStart, leftEnd, t);
            }

            if (rightDoor != null)
            {
                rightDoor.localRotation =
                    Quaternion.Slerp(rightStart, rightEnd, t);
            }

            yield return null;
        }

        if (leftDoor != null)
        {
            leftDoor.localRotation = leftEnd;
        }

        if (rightDoor != null)
        {
            rightDoor.localRotation = rightEnd;
        }

        HideSolvedObject();
    }
}