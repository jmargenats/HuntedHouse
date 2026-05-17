using UnityEngine;
using UnityEngine.SceneManagement;

public class RatCombatTrigger : MonoBehaviour
{
    [SerializeField] private string fightSceneName = "RatFight";

    private bool triggered = false;
    void Start()
    {
        triggered = false;
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance != null && GameManager.Instance.ratDefeated)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance.ignoreRatTriggerOnce)
        {
            GameManager.Instance.ignoreRatTriggerOnce = false;
            return;
        }

        if (triggered) return;

        triggered = true;

        GameManager.Instance.playerPosition =
            other.transform.position - other.transform.forward * 2f;

        GameManager.Instance.previousScene =
            SceneManager.GetActiveScene().name;

        StartCombat();
    }

    private void StartCombat()
    {
        Debug.Log("Combate iniciado con la rata");

        FindObjectOfType<SceneFader>().FadeAndLoadScene(fightSceneName);
    }

}