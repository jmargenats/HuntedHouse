using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCombatTrigger : MonoBehaviour
{
    public string enemyID;

    public string fightSceneName = "FightScene";

    private bool triggered;

    void Start()
    {
        if (
            GameManager.Instance.IsEnemyDefeated(
                enemyID
            )
        )
        {
            gameObject.SetActive(false);
        }
        Debug.Log("Enemy ID en escena: "+ enemyID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggered)
            return;

        triggered = true;

        GameManager.Instance.currentEnemyID =
            enemyID;

        GameManager.Instance.playerPosition =
            other.transform.position -
            other.transform.forward * 2f;

        GameManager.Instance.previousScene =
            SceneManager.GetActiveScene().name;

        FindObjectOfType<SceneFader>()
            .FadeAndLoadScene(fightSceneName);
    }
}