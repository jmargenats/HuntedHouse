using UnityEngine;

public class MapPickup : MonoBehaviour, IPickupable
{
    public bool CanPickup()
    {
        return !GameManager.Instance.hasMap;
    }

    public void Pickup()
    {
        GameManager.Instance.hasMap = true;

        PlayerInteractions player =
            FindFirstObjectByType<PlayerInteractions>();

        if (player != null)
        {
            player.ShowTemporaryMessage(
                "Presiona [M] para abrir el mapa",
                3f
            );
        }

        

        Destroy(gameObject);
    }
}