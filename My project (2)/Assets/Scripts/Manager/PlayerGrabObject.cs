using UnityEngine;

public class PlayerGrabObject : MonoBehaviour
{
    public float grabDistance = 300f;
    public Transform holdPoint;
    public float snapDistance = 0.5f;
    private GameObject heldObject;
    private Rigidbody heldRb;
    private float snapCooldown = 0f;

    private HighlightObject currentHighlight;
    void Update()
    {
        if (snapCooldown > 0)
            snapCooldown -= Time.deltaTime;

        CheckHighlight();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryGrab();
            else
                DropObject();
        }

        if (heldObject != null)
        {
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;

            TryAutoSnap();
        }
    }

    void TryGrab()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)
        );

        if (Physics.SphereCast(ray, 0.18f, out RaycastHit hit, grabDistance))
        {
            LetterCube cube = hit.collider.GetComponentInParent<LetterCube>();

            if (cube != null)
            {
                snapCooldown = 2f;
                if (cube.currentSlot != null)
                {
                    cube.currentSlot.currentCube = null;
                    cube.currentSlot = null;
                }
                heldObject = cube.gameObject;
                heldRb = heldObject.GetComponent<Rigidbody>();

                if (heldRb != null)
                {
                    heldRb.useGravity = false;
                    heldRb.isKinematic = true;
                }

                if (currentHighlight != null)
                {
                    currentHighlight.UnHighlight();
                    currentHighlight = null;
                }
            }
        }
    }
    void DropObject()
    {
        if (heldRb != null)
        {
            heldRb.isKinematic = false;
            heldRb.useGravity = true;
        }

        heldObject = null;
        heldRb = null;
    }

    void CheckHighlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)
        );

        if (Physics.SphereCast(ray, 0.15f, out RaycastHit hit, grabDistance))
        {
            

            HighlightObject highlight = hit.collider.GetComponentInParent<HighlightObject>();

            if (highlight != currentHighlight)
            {
                if (currentHighlight != null)
                    currentHighlight.UnHighlight();

                currentHighlight = highlight;

                if (currentHighlight != null)
                    currentHighlight.Highlight();
            }
        }
        else
        {
            if (currentHighlight != null)
            {
                currentHighlight.UnHighlight();
                currentHighlight = null;
            }
        }
    }
    void TryAutoSnap()
    {
        if (snapCooldown > 0) return;
        LetterCube cube = heldObject.GetComponent<LetterCube>();
        if (cube == null) return;

        GameObject[] slots = GameObject.FindGameObjectsWithTag("LetterSlot");

        foreach (GameObject slotObj in slots)
        {
            LetterSlot slot = slotObj.GetComponent<LetterSlot>();
            if (slot == null) continue;

            if (slot.currentCube != null && slot.currentCube != cube)
                continue;

            float distance = Vector3.Distance(
                heldObject.transform.position,
                slot.transform.position
            );

            if (distance <= snapDistance)
            {
                slot.currentCube = cube;
                cube.currentSlot = slot;

                heldObject.transform.position = slot.snapPoint.position;
                heldObject.transform.rotation = slot.snapPoint.rotation;

                if (heldRb != null)
                {
                    heldRb.isKinematic = true;
                    heldRb.useGravity = false;
                }

                heldObject = null;
                heldRb = null;

                Debug.Log("Cubo encajado en slot");
                return;
            }
        }
    }
}