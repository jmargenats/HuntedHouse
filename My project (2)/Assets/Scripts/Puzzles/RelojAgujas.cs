using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class ClockUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;

    public RectTransform hourHand;
    public RectTransform minuteHand;

    [Header("Player")]
    public PlayerInput playerInput;
    public FirstPersonController playerController;

    [Header("Hora inicial")]
    [Range(1, 12)]
    public int startHour = 3;

    [Range(0, 45)]
    public int startMinute = 15;

    private int currentHour;
    private int currentMinute;

    private bool opened = false;

    private ClockPuzzle currentPuzzle;

    public void OpenPuzzle(ClockPuzzle puzzle)
    {
        currentPuzzle = puzzle;

        currentHour = startHour;
        currentMinute = startMinute;

        opened = true;

        panel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (playerInput != null)
            playerInput.enabled = false;

        if (playerController != null)
            playerController.enabled = false;

        UpdateHands();
    }

    void Update()
    {
        if (!opened)
            return;

        // Hora izquierda
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            currentHour--;

            if (currentHour < 1)
                currentHour = 12;

            UpdateHands();
        }

        // Hora derecha
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            currentHour++;

            if (currentHour > 12)
                currentHour = 1;

            UpdateHands();
        }

        // Minutos izquierda
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            switch (currentMinute)
            {
                case 0:
                    currentMinute = 45;
                    break;

                case 15:
                    currentMinute = 0;
                    break;

                case 30:
                    currentMinute = 15;
                    break;

                case 45:
                    currentMinute = 30;
                    break;
            }

            UpdateHands();
        }

        // Minutos derecha
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            switch (currentMinute)
            {
                case 0:
                    currentMinute = 15;
                    break;

                case 15:
                    currentMinute = 30;
                    break;

                case 30:
                    currentMinute = 45;
                    break;

                case 45:
                    currentMinute = 0;
                    break;
            }

            UpdateHands();
        }

        // Confirmar
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            currentPuzzle.CheckTime(
                currentHour,
                currentMinute
            );

            ClosePuzzle();
        }
    }

    void UpdateHands()
    {
        // Hora
        hourHand.localEulerAngles =
            new Vector3(
                0,
                0,
                -(currentHour % 12) * 30f
            );

        // Minutos (solo 0,15,30,45)
        minuteHand.localEulerAngles =
            new Vector3(
                0,
                0,
                -(currentMinute * 6f)
            );

        Debug.Log(
            $"Reloj: {currentHour}:{currentMinute:00}"
        );
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