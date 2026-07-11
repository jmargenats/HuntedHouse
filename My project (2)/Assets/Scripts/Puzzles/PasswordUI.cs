using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using StarterAssets;

public class PasswordUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text[] slotTexts;

    public PlayerInput playerInput;
    public FirstPersonController playerController;

    private PasswordPuzzle currentPuzzle;
    private string currentInput = "";
    private bool opened = false;

    public void OpenPuzzle(PasswordPuzzle puzzle)
    {
        currentPuzzle = puzzle;
        currentInput = "";
        opened = true;

        panel.SetActive(true);
        UpdateText();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (playerInput != null)
            playerInput.enabled = false;

        if (playerController != null)
            playerController.enabled = false;
    }

    void Update()
    {
        if (!opened)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ClosePuzzle();
        }
    }

    public void PressLetter(string letter)
    {
        if (!opened || currentInput.Length >= slotTexts.Length)
            return;

        currentInput += letter.ToLower();
        UpdateText();
    }

    public void Backspace()
    {
        if (!opened || currentInput.Length == 0)
            return;

        currentInput =
            currentInput.Substring(
                0,
                currentInput.Length - 1
            );

        UpdateText();
    }

    public void Submit()
    {
        if (!opened || currentPuzzle == null)
            return;

        currentPuzzle.CheckPassword(currentInput);

        ClosePuzzle();
    }

    void UpdateText()
    {
        for (int i = 0; i < slotTexts.Length; i++)
        {
            slotTexts[i].text =
                i < currentInput.Length
                ? currentInput[i].ToString().ToUpper()
                : "";
        }
    }

    void ClosePuzzle()
    {
        opened = false;
        panel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerInput != null)
            playerInput.enabled = true;

        if (playerController != null)
            playerController.enabled = true;
    }
}