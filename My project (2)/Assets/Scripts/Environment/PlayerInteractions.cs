using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteractions : MonoBehaviour
{
    public float interactionDistance = 3f;

    public GameObject interactionText;

    void Update()
    {

        interactionText.SetActive(false);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            ItemRecolectable item = hit.collider.GetComponentInParent<ItemRecolectable>();
            Debug.Log("Estoy mirando: " + hit.collider.name);
            if (item != null)
            {
                Debug.Log("Tiene ItemRecolectable");
                interactionText.SetActive(true);
            }

            if (item != null)
            {
                interactionText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    item.Recolectar();
                }
            }
        }
    }
}
