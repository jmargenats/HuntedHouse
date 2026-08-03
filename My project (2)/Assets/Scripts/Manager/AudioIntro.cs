using UnityEngine;
using System.Collections;

public class AudioIntro : MonoBehaviour
{
    public AudioSource introSource;
    public AudioSource vientoSource;

    public float overlapSeconds = 2f;
    public float fadeSeconds = 2f;
    public float vientoTargetVolume = 0.7f;

    IEnumerator Start()
    {
        vientoSource.Stop();
        vientoSource.loop = true;
        vientoSource.playOnAwake = false;
        vientoSource.volume = 0f;

        introSource.loop = false;
        introSource.playOnAwake = false;

        if (
            GameManager.Instance != null &&
            GameManager.Instance.returningFromBattle
        )
        {
            
            introSource.Stop();

            // Retoma solamente el sonido ambiente.
            vientoSource.volume = vientoTargetVolume;
            vientoSource.Play();

            yield break;
        }
        introSource.Play();

        float waitTime = introSource.clip.length - overlapSeconds;
        if (waitTime > 0f)
            yield return new WaitForSeconds(waitTime);

        vientoSource.Play();

        float t = 0f;
        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            vientoSource.volume = Mathf.Lerp(0f, vientoTargetVolume, t / fadeSeconds);
            yield return null;
        }

        vientoSource.volume = vientoTargetVolume;
    }
}