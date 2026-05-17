using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public string enemyName = "Rat";

    public int maxHP = 30;
    public int currentHP;

    public int biteDamage = 5;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(enemyName + " recibió " + damage);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(enemyName + " murió");

        GameManager.Instance.ratDefeated = true;
    }
}