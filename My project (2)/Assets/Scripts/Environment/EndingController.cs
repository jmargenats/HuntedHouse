using UnityEngine;

public class EndingController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject badEnding;
    public GameObject neutralEnding;
    public GameObject goodEnding;

    [Header("Animators")]
    public Animator badAnimator;
    public Animator neutralAnimator;
    public Animator goodAnimator;

    [Header("Narraciones")]
    public AudioSource voiceSource;

    public AudioClip badVoice;
    public AudioClip neutralVoice;
    public AudioClip goodVoice;

    [Header("Música")]
    public AudioSource musicSource;

    void Start()
    {
        Debug.Log("playerDied: " + GameManager.Instance.playerDied);
        Debug.Log("helpedTomasEscape: " + GameManager.Instance.helpedTomasEscape);
        badEnding.SetActive(false);
        neutralEnding.SetActive(false);
        goodEnding.SetActive(false);

        // Música para todos los finales
        if (musicSource != null)
            musicSource.Play();

        // ==========================
        // FINAL MALO
        // ==========================

        if (GameManager.Instance.playerDied)
        {
            badEnding.SetActive(true);

            if (badAnimator != null)
                badAnimator.Play("BadEnding");

            if (voiceSource != null && badVoice != null)
                voiceSource.PlayOneShot(badVoice);

            return;
        }

        // ==========================
        // FINAL BUENO
        // ==========================

        if (GameManager.Instance.helpedTomasEscape)
        {
            goodEnding.SetActive(true);

            if (goodAnimator != null)
                goodAnimator.Play("GoodEnding");

            if (voiceSource != null && goodVoice != null)
                voiceSource.PlayOneShot(goodVoice);

            return;
        }

        // ==========================
        // FINAL NEUTRO
        // ==========================

        neutralEnding.SetActive(true);

        if (neutralAnimator != null)
            neutralAnimator.Play("NeutralEnding");

        if (voiceSource != null && neutralVoice != null)
            voiceSource.PlayOneShot(neutralVoice);
    }
}