using UnityEngine;
using UnityEngine.SceneManagement;

public class RatCombatTrigger : MonoBehaviour
{
    [SerializeField] private string fightSceneName = "FightScene";
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCombat();
        }
    }

    private void StartCombat()
    {
        Debug.Log("Combate iniciado con la rata");

        SceneManager.LoadScene(fightSceneName);
    }
}