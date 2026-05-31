using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Esquive")]
    [Range(0, 100)]
    public int dodgeChance = 25;

    [Header("Frenesí")]
    public int aggressiveThreshold = 10;

    public bool TryDodge()
    {
        return Random.Range(0, 100) < dodgeChance;
    }

    public int ChooseDamage(
        int baseDamage,
        int currentHP
    )
    {
        if (currentHP <= aggressiveThreshold)
        {
            Debug.Log(
                "El enemigo entra en frenesí"
            );

            return baseDamage + 3;
        }

        return baseDamage;
    }
}