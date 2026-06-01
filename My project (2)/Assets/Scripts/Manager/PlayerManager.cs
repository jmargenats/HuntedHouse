using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.returningFromBattle &&
            GameManager.Instance.playerPosition != Vector3.zero)
        {
            transform.position = GameManager.Instance.playerPosition;

            GameManager.Instance.returningFromBattle = false;
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