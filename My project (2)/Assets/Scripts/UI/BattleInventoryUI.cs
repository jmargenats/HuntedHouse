using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleInventoryUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text equippedWeaponText;

    [Header("Item Icons")]
    public Sprite shovelSprite;
    public Sprite knifeSprite;
    public Sprite pillsSprite;
    public Sprite medkitSprite;

    [Header("Inventory")]
    public Transform inventoryPanel;

    public GameObject inventoryButtonPrefab;

    private void Start()
    {
        GenerateInventoryButtons();

        UpdateUI();
    }
    public string GetSelectedItem()
    {
        return PlayerStats.Instance.equippedWeapon;
    }
    public void RefreshInventory()
    {
        GenerateInventoryButtons();

        UpdateUI();
    }

    void GenerateInventoryButtons()
    {
        // limpiar botones anteriores
        for (int i = inventoryPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryPanel.GetChild(i).gameObject);
        }

        HashSet<string> itemsAgregados = new HashSet<string>();

        foreach (string item in GameManager.Instance.collectedItems)
        {
            // ignorar items vacíos
            if (string.IsNullOrEmpty(item))
                continue;

            if (itemsAgregados.Contains(item))
                continue;
            
            itemsAgregados.Add(item);

            GameObject buttonObj =
                Instantiate(
                    inventoryButtonPrefab,
                    inventoryPanel
                );

            // ICONO
            Image icon =
                buttonObj.transform.Find("Icon")
                .GetComponent<Image>();

            switch (item)
            {
                case "Pala":
                    icon.sprite = shovelSprite;
                    break;

                case "Cuchillo":
                    icon.sprite = knifeSprite;
                    break;
                case "Pastillas":
                    icon.sprite = pillsSprite;
                    break;

                case "Botiquin":
                    icon.sprite = medkitSprite;
                    break;
            }

            // BOTON
            Button button =
                buttonObj.GetComponent<Button>();

            string currentItem = item;

            button.onClick.AddListener(() =>
            {
                EquipItem(currentItem);
            });
        }
    }

    void EquipItem(string itemName)
    {
        PlayerStats.Instance.EquipWeapon(itemName);

        UpdateUI();
    }

    void UpdateUI()
    {
        string weapon =
            PlayerStats.Instance.equippedWeapon;

        int damage =
            PlayerStats.Instance.GetWeaponDamage();

        if (string.IsNullOrEmpty(weapon))
        {
            equippedWeaponText.text =
                "Sin objeto equipado";
        }
        else
        {
            equippedWeaponText.text =
                weapon
                + "\nDaño: "
                + damage;
        }
    }
}