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
    public Sprite sedativeSprite;

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

                case "Sedante":
                    icon.sprite = sedativeSprite;
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
        string weapon = PlayerStats.Instance.equippedWeapon;

        if (string.IsNullOrEmpty(weapon))
        {
            equippedWeaponText.text =
                "Seleccioná un objeto para ver sus propiedades.";

            return;
        }

        switch (weapon)
        {
            case "Pala":

                equippedWeaponText.text =
                    "• Daño: " + PlayerStats.Instance.shovelDamage +
                    "\n• Impacto pesado." +
                    "\n• 15% de probabilidad de aturdir." +
                    "\n• Más difícil de esquivar.";

                break;

            case "Cuchillo":

                equippedWeaponText.text =
                    "• Daño: " + PlayerStats.Instance.knifeDamage +
                    "\n• 30% de probabilidad de sangrado." +
                    "\n• Más fácil de esquivar.";

                break;

            case "Pastillas":

                equippedWeaponText.text =
                    "• Recupera 20 HP." +
                    "\n• Se consume al usar.";

                break;

            case "Botiquin":

                equippedWeaponText.text =
                    "• Recupera 40 HP." +
                    "\n• Se consume al usar.";

                break;

            case "Sedante":

                equippedWeaponText.text =
                    "• No inflige daño." +
                    "\n• Reduce un 50% el daño de los próximos 3 ataques." +
                    "\n• Se consume al usar.";

                break;
    
            default:

                equippedWeaponText.text =
                    "Sin información.";

                break;
        }
    }
}