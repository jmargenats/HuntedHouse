using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractable
{
    public bool isOpen = false;

    public float openAngle = 90f;
    public float openSpeed = 2f;
    public SceneFader sceneFader;
    public string sceneToLoad;

    [Header("Llave")]
    public Inventario inventario;
    public string requiredItem = "Llave";
    public bool consumeKey = false;

    [Header("Diálogo")]
    public DialogueManager dialogueManager;
    public string lockedMessage = "Está cerrada. Necesito una llave.";

    private Quaternion closedRotation;
    private Quaternion openRotation;

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

    public void Interact()
    {
        TryOpenDoor();
    }

    void TryOpenDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            return;
        }

        string selectedItem = "";

        if (inventario != null)
        {
            selectedItem = inventario.DevolverItem();
        }

        if (selectedItem == requiredItem)
        {
            isOpen = true;
            StartCoroutine(EnterHouse());
            if (consumeKey)
            {
                inventario.UseSelectedItem();
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.collectedItems.Remove(requiredItem);
                }
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
    IEnumerator EnterHouse()
    {
        yield return new WaitForSeconds(1.5f);

        sceneFader.FadeAndLoadScene(sceneToLoad);
    }
}