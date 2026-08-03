using UnityEngine;

public class BearPickup : MonoBehaviour
{
    void Update()
    {
        if (
            GameManager.Instance != null &&
            GameManager.Instance.bearUnlocked &&
            !gameObject.activeSelf
        )
        {
            gameObject.SetActive(true);
        }
    }
}