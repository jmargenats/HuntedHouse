using UnityEngine;
using UnityEngine.SceneManagement;

public class RatCombatTrigger : MonoBehaviour
{
    [SerializeField] private string fightSceneName = "RatFight";

    private bool triggered = false;
    void Start()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.ratDefeated)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Evita retrigger instantáneo al volver
        if (GameManager.Instance.returningFromBattle)
        {
            GameManager.Instance.returningFromBattle = false;
            return;
        }

        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            GameManager.Instance.playerPosition =
                other.transform.position - other.transform.forward * 2f;

            GameManager.Instance.previousScene =
                SceneManager.GetActiveScene().name;

            StartCombat();
        }
    }

    private void StartCombat()
    {
        Debug.Log("Combate iniciado con la rata");

        FindObjectOfType<SceneFader>().FadeAndLoadScene(fightSceneName);
    }
}