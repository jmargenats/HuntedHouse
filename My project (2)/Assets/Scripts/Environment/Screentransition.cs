using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;

    [Header("Fade Settings")]
    public bool fadeOnStart = true;
    public bool fadeToBlack = false;

    public float fadeDuration = 1f;

    void Start()
    {
        if (fadeOnStart)
        {
            if (fadeToBlack)
            {
                StartCoroutine(Fade(0f, 1f));

            }
            else
            {
                StartCoroutine(Fade(1f, 0f));
            }
        }
    }

    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(Fade(0f, 1f));

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;

        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            color.a = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            fadeImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }
}