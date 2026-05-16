using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    public Inventario inventario;

    public Sprite iconoInventario;

    public string itemType;

    public void Recolectar()
    {
        inventario.AddItemToInventory(iconoInventario, itemType);

        gameObject.SetActive(false);
    }
}
