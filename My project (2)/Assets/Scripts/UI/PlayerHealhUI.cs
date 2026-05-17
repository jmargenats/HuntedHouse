using TMPro;
using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    public TMP_Text hpText;

    [Header("Barra de vida")]
    public int totalBars = 10;

    void Update()
    {
        UpdateHPUI();
    }

    void UpdateHPUI()
    {
        if (PlayerStats.Instance == null)
            return;

        int currentHP =
            PlayerStats.Instance.currentHP;

        int maxHP =
            PlayerStats.Instance.maxHP;

        // calcular cantidad de barras
        int barsToShow =
            Mathf.CeilToInt(
                ((float)currentHP / maxHP)
                * totalBars
            );

        // limitar
        barsToShow =
            Mathf.Clamp(
                barsToShow,
                0,
                totalBars
            );

        // construir barra
        string hpBar = "";

        for (int i = 0; i < barsToShow; i++)
        {
            hpBar += "█";
        }

        // color dinámico
        string hpColor = "#B00000";

        if (currentHP <= 25)
        {
            hpColor = "#FF0000";
        }

        // texto final
        hpText.text =
            "HP "
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