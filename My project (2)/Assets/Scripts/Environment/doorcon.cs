using UnityEngine;
using System.Collections;

public class doorcon : MonoBehaviour, IExaminable
{
    public AudioSource doorSound;
    public AudioClip doorOpen;
    public string doorstatus;

    public DialogueManager dialogueManager;

    [TextArea(2, 5)]
    public string lockedText = "La puerta está cerrada.";
    public string unlockedText = "La puerta está abierta.";

    [Header("Scene")]
    public bool changeScene = false;
    public SceneFader sceneFader;
    public string sceneToLoad;
    public float sceneDelay = 1.5f;

    [Header("Scene Requirement")]
    public Inventario inventario;
    public bool requireSelectedItemToChangeScene = false;
    public string requiredItem = "escalera";
    public string missingRequiredItemText =
        "Necesito una escalera para subir.";

    private bool isChangingScene = false;

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
            if (doorSound != null && doorOpen != null)
            {
                doorSound.clip = doorOpen;
                doorSound.Play();
            }

            if (changeScene)
            {
                TryChangeScene();
            }
            else
            {
                dialogueManager.ShowDialogue(unlockedText);
            }
        }
    }

    void TryChangeScene()
    {
        if (isChangingScene)
            return;

        if (!CanChangeScene())
            return;

        isChangingScene = true;

        StartCoroutine(ChangeSceneRoutine());
    }

    bool CanChangeScene()
    {
        if (!requireSelectedItemToChangeScene)
            return true;

        if (inventario == null)
        {
            dialogueManager.ShowDialogue(missingRequiredItemText);
            return false;
        }

        string selectedItem =
            inventario.DevolverItem();

        if (selectedItem == requiredItem)
            return true;

        dialogueManager.ShowDialogue(missingRequiredItemText);
        return false;
    }

    IEnumerator ChangeSceneRoutine()
    {
        if (!string.IsNullOrEmpty(unlockedText))
        {
            dialogueManager.ShowDialogue(unlockedText);
        }

        yield return new WaitForSeconds(sceneDelay);

        if (sceneFader != null)
        {
            sceneFader.FadeAndLoadScene(sceneToLoad);
        }
    }
}