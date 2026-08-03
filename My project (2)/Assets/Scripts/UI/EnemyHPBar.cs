using TMPro;
using UnityEngine;

public class EnemyHPUI : MonoBehaviour
{
    public TMP_Text hpText;

    public EnemyStats enemy;

    [Header("Barra de vida")]
    public int totalBars = 10;

    void Update()
    {
        UpdateHPUI();
    }

    void UpdateHPUI()
    {
        if (enemy == null)
            return;

        int currentHP =
            enemy.currentHP;

        int maxHP =
            enemy.maxHP;

        int barsToShow =
            Mathf.CeilToInt(
                ((float)currentHP / maxHP)
                * totalBars
            );

        barsToShow =
            Mathf.Clamp(
                barsToShow,
                0,
                totalBars
            );

        string hpBar = "";

        for (int i = 0; i < barsToShow; i++)
        {
            hpBar += "█";
        }

        string hpColor = "#B00000";

        if (currentHP <= maxHP / 4)
        {
            hpColor = "#FF0000";
        }

        hpText.text =
            enemy.enemyName.ToUpperInvariant()
            + "\n"
            + "HP "
            + currentHP
            + "/"
            + maxHP
            + "\n"
            + "<color="
            + hpColor
            + ">"
            + hpBar
            + "</color>";
    }
}