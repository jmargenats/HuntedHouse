using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;

    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("Llave")]
    public Inventario inventario;
    public string requiredItem = "Llave";
    public bool consumeKey = false;

    [Header("Diálogo")]
    public DialogueManager dialogueManager;
    public string lockedMessage = "Está cerrada. Necesito una llave.";

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private bool playerInRange = false;

    void Start()
    {
        closedRotation = transform.rotation;

        openRotation =
            Quaternion.Euler(
                transform.eulerAngles +
                new Vector3(0, openAngle, 0)
            );
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }

        Quaternion targetRotation =
            isOpen
            ? openRotation
            : closedRotation;

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * openSpeed
            );
    }

    void TryOpenDoor()
    {
        string selectedItem = "";

        if (inventario != null)
        {
            selectedItem = inventario.DevolverItem();
        }

        if (selectedItem == requiredItem)
        {
            isOpen = !isOpen;

            if (consumeKey)
            {
                inventario.UseSelectedItem();
            }
        }
        else
        {
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(lockedMessage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}