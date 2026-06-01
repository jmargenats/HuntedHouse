using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        if (
            GameManager.Instance != null &&
            GameManager.Instance.returningFromBattle &&
            GameManager.Instance.playerPosition != Vector3.zero
        )
        {
            CharacterController controller =
                GetComponent<CharacterController>();

            if (controller != null)
            {
                controller.enabled = false;
            }

            transform.position =
                GameManager.Instance.playerPosition;

            if (controller != null)
            {
                controller.enabled = true;
            }

            GameManager.Instance.returningFromBattle = false;

            Debug.Log(
                "Volví a posición guardada: " +
                transform.position
            );
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