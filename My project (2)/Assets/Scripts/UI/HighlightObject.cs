using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void Highlight()
    {
        transform.localScale = originalScale * 1.5f;
    }

    public void UnHighlight()
    {
        transform.localScale = originalScale;
    }
}