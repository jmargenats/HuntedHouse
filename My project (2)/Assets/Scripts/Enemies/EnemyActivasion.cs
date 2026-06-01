using UnityEngine;

public class EnemyActivationZone : MonoBehaviour
{
    public EnemyController[] enemies;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entró al trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Es el player, activo ratas");

            foreach (EnemyController enemy in enemies)
            {
                if (enemy != null)
                {
                    Debug.Log("Activo: " + enemy.name);
                    enemy.StartChasing();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (EnemyController enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.StopChasingAndReturn();
                }
            }
        }
    }
}