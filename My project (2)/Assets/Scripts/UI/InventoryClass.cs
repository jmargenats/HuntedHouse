using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public GameObject slotUI;
    public Image background;
    public Image itemIcon;
    public bool isSelected = false;
    public string itemType = "";
}