using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    public List<InventorySlot> slots;

    public Sprite defaultSlotSprite;

    private int currentSelectedSlot = 0;

    public List<InventoryItemData> itemDatabase;

    void Start()
    {
        foreach (var slot in slots)
        {
            slot.Initialize();
        }

        if (slots.Count > 0)
        {
            slots[currentSelectedSlot]
                .isSelected = true;
        }

        LoadInventoryFromGameManager();

        UpdateInventoryUI();
    }

    void Update()
    {
        HandleScrolling();
    }

    void HandleScrolling()
    {
        float scroll =
            Input.GetAxis(
                "Mouse ScrollWheel"
            );

        if (scroll < 0)
        {
            SelectNextSlot();
        }
        else if (scroll > 0)
        {
            SelectPreviousSlot();
        }
    }

    void SelectNextSlot()
    {
        if (slots.Count == 0)
            return;

        slots[currentSelectedSlot]
            .isSelected = false;

        currentSelectedSlot =
            (currentSelectedSlot + 1)
            % slots.Count;

        slots[currentSelectedSlot]
            .isSelected = true;

        UpdateInventoryUI();
    }

    void SelectPreviousSlot()
    {
        if (slots.Count == 0)
            return;

        slots[currentSelectedSlot]
            .isSelected = false;

        currentSelectedSlot =
            (
                currentSelectedSlot
                - 1
                + slots.Count
            )
            % slots.Count;

        slots[currentSelectedSlot]
            .isSelected = true;

        UpdateInventoryUI();
    }

    public void AddItemToInventory(
        Sprite itemSprite,
        string itemType,
        bool saveToGameManager = true
    )
    {

        foreach (var slot in slots)
        {
            if (
                string.IsNullOrEmpty(
                    slot.itemType
                )
            )
            {

                    Debug.Log("Slot: " + slot);

                    if (slot != null)
                    {
                        Debug.Log("ItemIcon: " + slot.itemIcon);
                        Debug.Log("ItemType: " + slot.itemType);
                    }

                    if (string.IsNullOrEmpty(slot.itemType))
                    {
                        slot.itemIcon.sprite = itemSprite;
                    }

                        slot.itemIcon.sprite =
                    itemSprite;

                slot.itemIcon.gameObject
                    .SetActive(true);

                slot.itemType =
                    itemType;

                if (
                    saveToGameManager &&
                    GameManager.Instance != null
                )
                {
                    GameManager.Instance
                        .AddItem(itemType);
                }

                if (
                    slot.background != null &&
                    defaultSlotSprite != null
                )
                {
                    slot.background.sprite =
                        defaultSlotSprite;
                }

                UpdateInventoryUI();

                return;
            }
        }

        Debug.Log(
            "No hay espacio en el inventario"
        );
    }

    public void UseSelectedItem()
    {
        if (slots.Count == 0)
            return;

        var selectedSlot =
            slots[currentSelectedSlot];

        ClearSlot(selectedSlot);
    }

    void ClearSlot(
        InventorySlot slot
    )
    {
        slot.itemIcon.sprite =
            null;

        slot.itemIcon.gameObject
            .SetActive(false);

        slot.itemType = "";

        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        foreach (var slot in slots)
        {
            if (slot.slotUI != null)
            {
                Image slotImage =
                    slot.slotUI
                    .GetComponent<Image>();

                if (slotImage != null)
                {
                    Color color =
                        slotImage.color;

                    color.a =
                        slot.isSelected
                        ? 1f
                        : 0.4f;

                    slotImage.color =
                        color;
                }
            }
        }
    }
    public void RemoveItem(string itemType)
    {
        foreach (var slot in slots)
        {
            if (slot.itemType == itemType)
            {
                slot.itemIcon.sprite = null;

                slot.itemIcon.gameObject
                    .SetActive(false);

                slot.itemType = "";

                break;
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance
                .collectedItems
                .Remove(itemType);
        }

        UpdateInventoryUI();
    }
    public string DevolverItem()
    {
        if (slots.Count == 0)
            return "";

        var selectedSlot =
            slots[currentSelectedSlot];

        Debug.Log(
            selectedSlot.itemType
        );

        return selectedSlot.itemType;
    }

    void LoadInventoryFromGameManager()
    {
        if (
            GameManager.Instance == null
        )
            return;

        foreach (
            string itemType
            in GameManager.Instance
                .collectedItems
        )
        {
            Sprite icon =
                GetIconForItem(
                    itemType
                );

            if (icon != null)
            {
                AddItemToInventory(
                    icon,
                    itemType,
                    false
                );
            }
        }
    }

    Sprite GetIconForItem(
        string itemType
    )
    {
        foreach (
            var item
            in itemDatabase
        )
        {
            if (
                item.itemType
                == itemType
            )
            {
                return item.icon;
            }
        }

        return null;
    }
}