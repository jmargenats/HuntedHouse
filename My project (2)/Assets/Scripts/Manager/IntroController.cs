using UnityEngine;
using System.Collections;
using StarterAssets;

public class IntroController : MonoBehaviour
{
    public SceneFader sceneFader;
    public DialogueManager dialogueManager;

    private FirstPersonController playerMovement;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.returningFromBattle)
        {
            return;
        }
        Color c = sceneFader.fadeImage.color;
        c.a = 1f;
        sceneFader.fadeImage.color = c;

        playerMovement = FindFirstObjectByType<FirstPersonController>();

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        playerMovement.enabled = false;

        // Pantalla negra inicial
        yield return new WaitForSeconds(6f);

        // Ahora aparece la imagen
        yield return StartCoroutine(sceneFader.Fade(1f, 0f));

        playerMovement.enabled = true;

        yield return new WaitForSeconds(0.5f);

        dialogueManager.ShowDialogue("Al fin llegué...");

        yield return new WaitForSeconds(3f);

        dialogueManager.ShowDialogue("La casa del doctor Octavio Ferrer.");

        yield return new WaitForSeconds(3f);

        dialogueManager.ShowDialogue("Hace años que nadie vive acá...");

        yield return new WaitForSeconds(3f);

        dialogueManager.ShowDialogue("O al menos eso dicen.");
    }
}