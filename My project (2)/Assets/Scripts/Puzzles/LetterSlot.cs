using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public Transform snapPoint;
    public LetterCube currentCube;

    void Awake()
    {
        if (snapPoint == null)
            snapPoint = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        LetterCube cube = other.GetComponentInParent<LetterCube>();

        if (cube != null && currentCube == null)
        {
            currentCube = cube;
            cube.currentSlot = this;

            SnapCube();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LetterCube cube = other.GetComponentInParent<LetterCube>();

        if (cube != null && cube == currentCube)
        {
            currentCube = null;
            cube.currentSlot = null;
        }
    }

    public void SnapCube()
    {
        if (currentCube == null) return;

        currentCube.transform.position = snapPoint.position;
        currentCube.transform.rotation = snapPoint.rotation;

        Rigidbody rb = currentCube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}