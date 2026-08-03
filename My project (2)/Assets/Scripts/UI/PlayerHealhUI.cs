using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("Texto")]
    public TMP_Text hpText;

    [Header("Barra de vida")]
    public int totalBars = 10;

    [Header("Retrato")]
    public Image portraitImage;

    public Sprite greenPortrait;
    public Sprite orangePortrait;
    public Sprite redPortrait;

    void Start()
    {
        UpdateHPUI();
    }

    void Update()
    {
        UpdateHPUI();
    }

    void UpdateHPUI()
    {
        if (PlayerStats.Instance == null)
            return;

        int currentHP = PlayerStats.Instance.currentHP;
        int maxHP = PlayerStats.Instance.maxHP;

        //-------------------------
        // Retrato
        //-------------------------

        if (portraitImage != null)
        {
            float hpPercent =
                (float)currentHP / maxHP;

            if (hpPercent <= 0.3f)
            {
                portraitImage.sprite = redPortrait;
            }
            else if (hpPercent <= 0.6f)
            {
                portraitImage.sprite = orangePortrait;
            }
            else
            {
                portraitImage.sprite = greenPortrait;
            }
        }

        //-------------------------
        // Barras
        //-------------------------

        int barsToShow = Mathf.CeilToInt(
            ((float)currentHP / maxHP) * totalBars
        );

        barsToShow = Mathf.Clamp(
            barsToShow,
            0,
            totalBars
        );

        string hpBar = "";

        for (int i = 0; i < barsToShow; i++)
        {
            hpBar += "█";
        }

        //-------------------------
        // Color
        //-------------------------

        string hpColor;

        float percent = (float)currentHP / maxHP;

        if (percent <= 0.3f)
        {
            hpColor = "#FF0000";
        }
        else if (percent <= 0.6f)
        {
            hpColor = "#FFA500";
        }
        else
        {
            hpColor = "#00FF66";
        }

        //-------------------------
        // Texto
        //-------------------------

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