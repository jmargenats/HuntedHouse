using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Dodge")]
    [Range(0, 100)]
    public int dodgeChance = 25;

    [Header("Aggressive")]
    public int aggressiveThreshold = 10;

    public bool TryDodge()
    {
        int roll = Random.Range(0, 100);

        return roll < dodgeChance;
    }

    public int ChooseDamage(int baseDamage, int currentHP)
    {
        // si está herida se vuelve agresiva
        if (currentHP <= aggressiveThreshold)
        {
            Debug.Log("La rata entra en frenesí");

            return baseDamage + 3;
        }

        return baseDamage;
    }
}