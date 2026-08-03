using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        GameManager gameManager =
            GameManager.Instance;

        CharacterController controller =
            GetComponent<CharacterController>();

        // Posición configurada por una puerta específica
        if (
            gameManager != null &&
            gameManager.hasPendingSceneSpawn
        )
        {
            if (controller != null)
            {
                controller.enabled = false;
            }

            transform.SetPositionAndRotation(
                gameManager.pendingSpawnPosition,
                Quaternion.Euler(
                    gameManager.pendingSpawnRotation
                )
            );

            if (controller != null)
            {
                controller.enabled = true;
            }

            // El spawn se utiliza una sola vez
            gameManager.hasPendingSceneSpawn = false;
            gameManager.returningFromBattle = false;

            Debug.Log(
                "Aparecí en la posición configurada por la puerta: " +
                transform.position
            );
        }
        // Comportamiento anterior al regresar de una batalla
        else if (
            gameManager != null &&
            gameManager.returningFromBattle &&
            gameManager.playerPosition != Vector3.zero
        )
        {
            if (controller != null)
            {
                controller.enabled = false;
            }

            transform.position =
                gameManager.playerPosition;

            if (controller != null)
            {
                controller.enabled = true;
            }

            gameManager.returningFromBattle = false;

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