using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractable
{
    public enum DoorRequirement
    {
        None,
        Item,
        EnemyDefeated
    }

    [Header("Lock")]
    public GameObject lockObject;
    [Header("Examine")]
    public doorcon examineDoor;
    [Header("Door")]
    public bool isOpen = false;
    [Header("Save")]
    public bool saveLockState = false;

    public float openAngle = 90f;
    public float openSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    [Header("Requirement")]
    public DoorRequirement requirement = DoorRequirement.None;

    [Header("Item")]
    public Inventario inventario;
    public string requiredItem = "Llave";
    public bool consumeKey = false;
    public bool requireSelectedItem = true;

    [Header("Enemy")]
    public string requiredEnemyID;

    [Header("Scene")]
    public bool changeScene = false;
    public SceneFader sceneFader;
    public string sceneToLoad;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    public string lockedMessage =
        "Est\u00E1 cerrada.";

    [TextArea(2, 4)]
    public string[] transitionMessages;

    [Min(0f)]
    public float sceneTransitionDelay = 1.5f;

    private bool transitionInProgress;

    void Start()
    {
        closedRotation = transform.rotation;

        openRotation =
            Quaternion.Euler(
                transform.eulerAngles +
                new Vector3(0, openAngle, 0)
            );
        if (
            saveLockState &&
            GameManager.Instance != null &&
            GameManager.Instance.screwdriverLockRemoved
        )
        {
            if (lockObject != null)
                Destroy(lockObject);

            if (examineDoor != null)
                examineDoor.doorstatus = "unlock";

            requirement = DoorRequirement.None;
        }
    }

    void Update()
    {
        Quaternion target =
            isOpen
            ? openRotation
            : closedRotation;

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                target,
                Time.deltaTime * openSpeed
            );
    }

    public void Interact()
    {
        if (transitionInProgress)
            return;

        if (isOpen)
        {
            isOpen = false;
            return;
        }

        switch (requirement)
        {
            case DoorRequirement.None:

                OpenDoor();

                break;

            case DoorRequirement.Item:

                TryOpenWithItem();

                break;

            case DoorRequirement.EnemyDefeated:

                TryOpenAfterEnemy();

                break;
        }
    }

    void OpenDoor()
    {
        isOpen = true;

        if (
            saveLockState &&
            GameManager.Instance != null
        )
        {
            GameManager.Instance.screwdriverLockRemoved = true;
            requirement = DoorRequirement.None;
            GameManager.Instance.SaveGame();
        }

        if (lockObject != null)
        {
            Destroy(lockObject);
        }

        if (examineDoor != null)
        {
            examineDoor.doorstatus = "unlock";
        }

        if (changeScene)
        {
            transitionInProgress = true;

            StartCoroutine(
                EnterScene()
            );
        }
    }

    void TryOpenWithItem()
    {
        string selectedItem = inventario != null
            ? inventario.DevolverItem()
            : string.Empty;

        bool hasRequiredItem =
            selectedItem == requiredItem;

        if (
            !requireSelectedItem &&
            GameManager.Instance != null
        )
        {
            hasRequiredItem =
                hasRequiredItem ||
                GameManager.Instance.HasItem(requiredItem);
        }

        if (hasRequiredItem)
        {
            OpenDoor();

            if (consumeKey)
            {
                inventario.RemoveItem(requiredItem);

                GameManager.Instance.collectedItems.Remove(requiredItem);

                if (requiredItem == "screwdriver")
                {
                    GameManager.Instance.screwdriverUsed = true;

                    GameManager.Instance.SaveGame();
                }
            }
        }
        else
        {
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(
                    lockedMessage
                );
            }
        }
    }

    void TryOpenAfterEnemy()
    {
        if (
            GameManager.Instance != null &&
            GameManager.Instance.IsEnemyDefeated(requiredEnemyID)
        )
        {
            OpenDoor();
        }
        else
        {
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(
                    lockedMessage
                );
            }
        }
    }

    IEnumerator EnterScene()
    {
        bool showedMessage = false;

        if (
            dialogueManager != null &&
            transitionMessages != null
        )
        {
            foreach (string message in transitionMessages)
            {
                if (string.IsNullOrWhiteSpace(message))
                    continue;

                showedMessage = true;
                dialogueManager.ShowDialogue(message);

                yield return new WaitForSeconds(
                    dialogueManager.displayTime
                );
            }
        }

        if (!showedMessage && sceneTransitionDelay > 0f)
        {
            yield return new WaitForSeconds(
                sceneTransitionDelay
            );
        }

        if (
            sceneFader == null ||
            string.IsNullOrWhiteSpace(sceneToLoad)
        )
        {
            Debug.LogWarning(
                $"La puerta {name} no tiene configurado el SceneFader o la escena de destino.",
                this
            );

            transitionInProgress = false;
            yield break;
        }

        sceneFader.FadeAndLoadScene(
            sceneToLoad
        );
    }
}
