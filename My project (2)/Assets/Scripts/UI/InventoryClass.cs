using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public GameObject slotUI;

    [HideInInspector]
    public Image background;

    [HideInInspector]
    public Image itemIcon;

    public bool isSelected = false;

    public string itemType = "";

    public void Initialize()
    {
        if (slotUI == null)
            return;

        background =
            slotUI.GetComponent<Image>();

        Transform iconTransform =
            slotUI.transform.Find("ItemIcon");

        if (iconTransform != null)
        {
            itemIcon =
                iconTransform.GetComponent<Image>();
        }
    }
}

[System.Serializable]
public class InventoryItemData
{
    public string itemType;
    public Sprite icon;
}