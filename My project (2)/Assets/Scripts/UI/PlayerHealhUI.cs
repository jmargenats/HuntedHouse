using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHPUI : MonoBehaviour
{
    public TMP_Text hpText;

    [Header("Barra de vida")]
    public int totalBars = 10;
    public TMP_Text strengthText;
    [Header("Icono de vida")]
    public Image portraitImage;

    public Sprite greenPortrait;
    public Sprite orangePortrait;
    public Sprite redPortrait;

    void Update()
    {
        UpdateHPUI();
        strengthText.text =
    "FUE "
    + PlayerStats.Instance.strength
    + " ("
    + PlayerStats.Instance.fistHits
    + "/5)";
    }

    void UpdateHPUI()
    {
        if (PlayerStats.Instance == null)
            return;

        int currentHP =
            PlayerStats.Instance.currentHP;
        if (portraitImage != null)
        {
            if (currentHP < 30)
            {
                portraitImage.sprite =
                    redPortrait;
            }
            else if (currentHP < 60)
            {
                portraitImage.sprite =
                    orangePortrait;
            }
            else
            {
                portraitImage.sprite =
                    greenPortrait;
            }
        }
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

        if (currentHP < 30)
        {
            hpColor = "#FF0000";
        }
        else if (currentHP < 60)
        {
            hpColor = "#FFA500";
        }
        else
        {
            hpColor = "#00FF66";
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