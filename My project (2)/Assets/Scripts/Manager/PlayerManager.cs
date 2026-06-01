using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.playerPosition != Vector3.zero)
            {
                transform.position = GameManager.Instance.playerPosition;
            }
        }
        if (PlayerStats.Instance != null)
        {
            Debug.Log(
                "HP al volver a la casa: " +
                PlayerStats.Instance.currentHP
            );
        }
    }
}