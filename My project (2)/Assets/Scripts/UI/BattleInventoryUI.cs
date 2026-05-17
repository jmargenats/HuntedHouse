using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleInventoryUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text equippedWeaponText;

    [Header("Item Icons")]
    public Sprite shovelSprite;
    public Sprite knifeSprite;

    [Header("Inventory")]
    public Transform inventoryPanel;

    public GameObject inventoryButtonPrefab;

    private void Start()
    {
        GenerateInventoryButtons();

        UpdateUI();
    }

    void GenerateInventoryButtons()
    {
        // limpiar botones anteriores
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (string item in GameManager.Instance.collectedItems)
        {
            // ignorar items vacíos
            if (string.IsNullOrEmpty(item))
                continue;

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