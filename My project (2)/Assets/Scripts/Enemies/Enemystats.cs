using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Info")]
    public string enemyID;

    public string enemyName;

    [TextArea]
    public string enemyDescription;

    [Header("Stats")]
    public int maxHP = 30;

    public int currentHP;

    public int attackDamage = 5;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(
            enemyName +
            " recibió " +
            damage +
            " de daño"
        );

        if (currentHP <= 0)
        {
            currentHP = 0;

            Die();
        }
    }

    void Die()
    {
        Debug.Log(enemyName + " murió");
    }
}