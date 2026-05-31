using UnityEngine;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    public float interactionDistance = 3f;

    public TMP_Text interactionText;

    void Update()
    {
        interactionText.gameObject.SetActive(false);

        Ray ray =
            Camera.main.ViewportPointToRay(
                new Vector3(0.5f, 0.5f, 0)
            );

        RaycastHit hit;

        if (
            Physics.Raycast(
                ray,
                out hit,
                interactionDistance
            )
        )
        {
            // Sistema viejo
            ItemRecolectable item =
                hit.collider
                .GetComponentInParent<ItemRecolectable>();

            // Sistema nuevo
            IInteractable interactable =
                hit.collider
                .GetComponentInParent<IInteractable>();

            IPickupable pickupable =
                hit.collider
                .GetComponentInParent<IPickupable>();

            if (
                item != null ||
                interactable != null ||
                pickupable != null
            )
            {
                interactionText.gameObject.SetActive(true);

                string prompt = "";

                // F = Examinar
                if (interactable != null)
                {
                    prompt += "[F] Examinar";
                }

                // E = Recoger (nuevo sistema)
                if (
                    pickupable != null &&
                    pickupable.CanPickup()
                )
                {
                    if (prompt != "")
                    {
                        prompt += "\n";
                    }

                    prompt += "[E] Recoger";
                }

                // E = Sistema viejo
                if (
                    item != null &&
                    interactable == null &&
                    pickupable == null
                )
                {
                    prompt = "[E] Recoger";
                }

                interactionText.text = prompt;
            }

            // =========================
            // F = INTERACTUAR
            // =========================

            if (
                interactable != null &&
                Input.GetKeyDown(KeyCode.F)
            )
            {
                interactable.Interact();
            }

            // =========================
            // E = RECOGER (nuevo)
            // =========================

            if (
                pickupable != null &&
                pickupable.CanPickup() &&
                Input.GetKeyDown(KeyCode.E)
            )
            {
                pickupable.Pickup();
            }

            // =========================
            // E = RECOGER (viejo)
            // =========================

            else if (
                item != null &&
                Input.GetKeyDown(KeyCode.E)
            )
            {
                item.Recolectar();
            }
        }
    }
}