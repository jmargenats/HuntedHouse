using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Esquive")]
    [Range(0, 100)]
    public int dodgeChance = 20;

    [Header("Frenesí")]
    public int aggressiveThreshold = 10;

    public bool TryDodge(string weapon)
    {
        int chance = dodgeChance;

        switch (weapon)
        {
            case "Puño":

                chance += 0;

                break;

            case "Pala":

                chance -= 8;

                break;

            case "Cuchillo":

                chance += 8;

                break;
        }

        chance = Mathf.Clamp(
            chance,
            0,
            95);

        return Random.Range(0, 100) < chance;
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