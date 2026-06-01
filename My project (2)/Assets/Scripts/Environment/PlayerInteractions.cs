using UnityEngine;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    public float interactionDistance = 1f;

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
            // =========================
            // COMPONENTES
            // =========================

            ItemRecolectable item =
                hit.collider
                .GetComponentInParent<ItemRecolectable>();

            IInteractable interactable =
                hit.collider
                .GetComponentInParent<IInteractable>();

            IExaminable examinable =
                hit.collider
                .GetComponentInParent<IExaminable>();

            IPickupable pickupable =
                hit.collider
                .GetComponentInParent<IPickupable>();

            // =========================
            // TEXTO DE INTERACCIÓN
            // =========================

            if (
                item != null ||
                interactable != null ||
                examinable != null ||
                pickupable != null
            )
            {
                interactionText.gameObject
                    .SetActive(true);

                string prompt = "";

                // F = Examinar / Interactuar
                if (
                    interactable != null ||
                    examinable != null
                )
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
                    examinable == null &&
                    pickupable == null
                )
                {
                    prompt = "[E] Recoger";
                }

                interactionText.text =
                    prompt;
            }

            // =========================
            // F = PUZZLES
            // =========================

            if (
                interactable != null &&
                Input.GetKeyDown(KeyCode.F)
            )
            {
                interactable.Interact();
            }

            // =========================
            // F = EXAMINAR
            // =========================

            if (
                examinable != null &&
                Input.GetKeyDown(KeyCode.F)
            )
            {
                examinable.Examine();
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