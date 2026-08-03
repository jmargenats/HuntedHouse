using UnityEngine;

using System.Collections;

public class SofiaBodyDialogue : MonoBehaviour, IInteractable
{
    public DialogueManager dialogueManager;

    [Header("Informes necesarios")]
    public NewspaperInteractable informeSofia;
    public NewspaperInteractable informeFamiliar;

    [Header("Antes de leer los informes")]
    [TextArea]
    public string[] lockedDialogues;

    [Header("Después de leer ambos informes")]
    [TextArea]
    public string[] unlockedDialogues;

    [Header("Audio al desbloquear el diálogo")]
    public AudioClip firstUnlockedAudio;
    public AudioClip secondUnlockedAudio;
    [Min(0f)] public float delayBetweenAudios = 1f;

    [Header("Encuentro con el científico")]
    public GameObject scientistEnemy;
    public GameObject scientistActivationZone;

    private AudioSource voiceSource;
    private Coroutine audioSequence;

    public void Interact()
    {
        bool bothReportsRead =
            informeSofia != null &&
            informeFamiliar != null &&
            informeSofia.HasBeenRead &&
            informeFamiliar.HasBeenRead;

        string[] currentDialogues =
            bothReportsRead
                ? unlockedDialogues
                : lockedDialogues;

        if (dialogueManager != null &&
            currentDialogues != null &&
            currentDialogues.Length > 0)
        {
            int randomIndex = Random.Range(0, currentDialogues.Length);

            dialogueManager.ShowDialogue(
                currentDialogues[randomIndex]
            );
        }

        if (bothReportsRead)
        {
            ActivateScientistEncounter();

            if (audioSequence == null)
                audioSequence = StartCoroutine(PlayUnlockedAudioSequence());
        }
    }

    private void ActivateScientistEncounter()
    {
        if (scientistEnemy != null)
            scientistEnemy.SetActive(true);

        if (scientistActivationZone != null)
            scientistActivationZone.SetActive(true);
    }

    private IEnumerator PlayUnlockedAudioSequence()
    {
        EnsureVoiceSource();

        if (firstUnlockedAudio != null)
        {
            voiceSource.PlayOneShot(firstUnlockedAudio);
            yield return new WaitForSeconds(firstUnlockedAudio.length);
        }

        if (firstUnlockedAudio != null && secondUnlockedAudio != null)
            yield return new WaitForSeconds(delayBetweenAudios);

        if (secondUnlockedAudio != null)
        {
            voiceSource.PlayOneShot(secondUnlockedAudio);
            yield return new WaitForSeconds(secondUnlockedAudio.length);
        }

        audioSequence = null;
    }

    private void EnsureVoiceSource()
    {
        if (voiceSource != null)
            return;

        voiceSource = GetComponent<AudioSource>();

        if (voiceSource == null)
            voiceSource = gameObject.AddComponent<AudioSource>();

        voiceSource.playOnAwake = false;
        voiceSource.loop = false;
        voiceSource.spatialBlend = 0f;
    }
}
