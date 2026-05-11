using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("Base Fog Settings")]
    [SerializeField] private Color fogColor = new Color(0.45f, 0.45f, 0.45f);
    [SerializeField] private float minDensity = 0.05f;
    [SerializeField] private float maxDensity = 0.5f;

    [Header("Current State")]
    [Range(0f, 1f)]
    [SerializeField] private float intensity = 0f;

    private void Awake()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = fogColor;
    }

    private void Update()
    {
        RenderSettings.fogDensity = Mathf.Lerp(minDensity, maxDensity, intensity);
    }


    public void SetIntensity(float value)
    {
        intensity = Mathf.Clamp01(value);
    }

    public float GetIntensity()
    {
        return intensity;
    }
}