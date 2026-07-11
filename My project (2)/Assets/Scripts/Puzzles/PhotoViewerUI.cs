using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PhotoViewerUI : MonoBehaviour
{
    public static bool IsOpen = false;

    public GameObject panel;
    public Image photoImage;
    public TMP_Text instructionsText;
    public string instructions =
        "F para salir\nEspacio para girar";

    [Header("Player")]
    public PlayerInput playerInput;
    public FirstPersonController playerController;

    private Sprite frontSprite;
    private Sprite backSprite;
    private bool showingBack = false;
    private bool opened = false;
    private PlayerInteractions playerInteractions;

    public void OpenPhoto(Sprite front, Sprite back)
    {
        frontSprite = front;
        backSprite = back;
        showingBack = false;
        opened = true;
        IsOpen = true;

        if (panel != null)
        {
            panel.SetActive(true);
        }

        if (instructionsText != null)
        {
            instructionsText.text = instructions;
        }

        UpdatePhoto();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (playerInput != null)
            playerInput.enabled = false;

        if (playerController != null)
            playerController.enabled = false;

        playerInteractions =
            FindObjectOfType<PlayerInteractions>();

        if (playerInteractions != null)
            playerInteractions.enabled = false;
    }

    void Update()
    {
        if (!opened)
            return;

        if (
            Keyboard.current.escapeKey.wasPressedThisFrame ||
            Keyboard.current.fKey.wasPressedThisFrame
        )
        {
            ClosePhoto();
        }

        if (
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.rKey.wasPressedThisFrame
        )
        {
            FlipPhoto();
        }
    }

    public void FlipPhoto()
    {
        if (!opened || backSprite == null)
            return;

        showingBack = !showingBack;
        UpdatePhoto();
    }

    public void ClosePhoto()
    {
        opened = false;
        IsOpen = false;

        if (panel != null)
        {
            panel.SetActive(false);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerInput != null)
            playerInput.enabled = true;

        if (playerController != null)
            playerController.enabled = true;

        if (playerInteractions != null)
            playerInteractions.enabled = true;
    }

    void UpdatePhoto()
    {
        if (photoImage == null)
            return;

        photoImage.sprite =
            showingBack && backSprite != null
                ? backSprite
                : frontSprite;
    }
}
