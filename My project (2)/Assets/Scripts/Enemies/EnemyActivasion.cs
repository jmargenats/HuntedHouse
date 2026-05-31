using UnityEngine;

public class EnemyActivationZone : MonoBehaviour
{
    public EnemyController enemy;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            enemy.StartChasing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.StopChasingAndReturn();
        }
    }
}