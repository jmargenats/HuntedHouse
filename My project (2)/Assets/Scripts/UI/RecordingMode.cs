using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;

public class RecordingMode : MonoBehaviour
{
    public GameObject recordingUI;
    public TMP_Text timerText;
    public Light flashlight;
    public TMP_Text recText;

    public GameObject staticOverlay;
    public VideoPlayer staticVideo;

    private Coroutine blinkCoroutine;
    private Coroutine staticCoroutine;

    private bool isRecording = false;
    private float recordingTime = 0f;

    void Start()
    {
        if (flashlight != null)
            flashlight.enabled = false;

        if (recordingUI != null)
            recordingUI.SetActive(false);

        if (staticOverlay != null)
            staticOverlay.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleRecording();
        }

        if (isRecording)
        {
            recordingTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(recordingTime / 60f);
            int seconds = Mathf.FloorToInt(recordingTime % 60f);

            if (timerText != null)
            {
                timerText.text =
                    minutes.ToString("00") + ":" +
                    seconds.ToString("00");
            }
        }
    }

    void ToggleRecording()
    {
        isRecording = !isRecording;

        recordingUI.SetActive(isRecording);

        if (flashlight != null)
            flashlight.enabled = isRecording;

        if (isRecording)
        {
            recordingTime = 0f;

            recText.gameObject.SetActive(true);

            blinkCoroutine = StartCoroutine(BlinkREC());

            StartCoroutine(StartupStatic());

            staticCoroutine = StartCoroutine(RandomStatic());
        }
        else
        {
            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);

            if (staticCoroutine != null)
                StopCoroutine(staticCoroutine);

            recText.gameObject.SetActive(true);

            if (staticVideo != null)
                staticVideo.Stop();

            if (staticOverlay != null)
                staticOverlay.SetActive(false);
        }
    }

    IEnumerator BlinkREC()
    {
        while (isRecording)
        {
            recText.gameObject.SetActive(
                !recText.gameObject.activeSelf
            );

            yield return new WaitForSeconds(
                Random.Range(0.4f, 0.7f)
            );
        }
    }

    IEnumerator RandomStatic()
    {
        while (isRecording)
        {
            yield return new WaitForSeconds(
                Random.Range(5f, 10f)
            );

            if (!isRecording) yield break;

            staticOverlay.SetActive(true);

            staticVideo.Stop();
            staticVideo.Play();

            float duration =
                staticVideo.length > 0
                ? (float)staticVideo.length
                : 0.5f;

            yield return new WaitForSeconds(duration);

            staticOverlay.SetActive(false);
        }
    }
    IEnumerator StartupStatic()
    {
        staticOverlay.SetActive(true);

        staticVideo.Play();

        yield return new WaitForSeconds(0.3f);

        staticVideo.Stop();

        staticOverlay.SetActive(false);
    }
}