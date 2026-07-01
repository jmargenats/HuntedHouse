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

    [Header("Door")]
    public bool isOpen = false;

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

    [Header("Enemy")]
    public string requiredEnemyID;

    [Header("Scene")]
    public bool changeScene = false;
    public SceneFader sceneFader;
    public string sceneToLoad;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    public string lockedMessage =
        "Está cerrada.";

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

        if (changeScene)
        {
            StartCoroutine(
                EnterScene()
            );
        }
    }

    void TryOpenWithItem()
    {
        if (inventario == null)
            return;

        string selectedItem =
            inventario.DevolverItem();

        if (selectedItem == requiredItem)
        {
            OpenDoor();

            if (consumeKey)
            {
                inventario.UseSelectedItem();

                GameManager.Instance
                    .collectedItems
                    .Remove(requiredItem);
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
        yield return new WaitForSeconds(1.5f);

        sceneFader.FadeAndLoadScene(
            sceneToLoad
        );
    }
}