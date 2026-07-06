using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class ClockUI : MonoBehaviour
{
    public static bool IsOpen = false;

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

    private Quaternion hourStartRotation;
    private Quaternion minuteStartRotation;

    void Start()
    {
        hourStartRotation = hourHand.localRotation;
        minuteStartRotation = minuteHand.localRotation;

        Debug.Log("CLOCKUI START");
    }

    public void OpenPuzzle(ClockPuzzle puzzle)
    {
        IsOpen = true;

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

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            currentHour--;

            if (currentHour < 1)
                currentHour = 12;

            UpdateHands();
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            currentHour++;

            if (currentHour > 12)
                currentHour = 1;

            UpdateHands();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            switch (currentMinute)
            {
                case 0: currentMinute = 45; break;
                case 15: currentMinute = 0; break;
                case 30: currentMinute = 15; break;
                case 45: currentMinute = 30; break;
            }

            UpdateHands();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            switch (currentMinute)
            {
                case 0: currentMinute = 15; break;
                case 15: currentMinute = 30; break;
                case 30: currentMinute = 45; break;
                case 45: currentMinute = 0; break;
            }

            UpdateHands();
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log($"ENVIANDO -> {currentHour}:{currentMinute:00}");

            currentPuzzle.CheckTime(
                currentHour,
                currentMinute
            );

            Debug.Log("VOLVI DE CHECKTIME");

            ClosePuzzle();
        }
    }

    void UpdateHands()
    {
        float hourOffset =
            (currentHour - startHour) * -30f;

        float minuteOffset =
            (currentMinute - startMinute) * -6f;

        hourHand.localRotation =
            hourStartRotation *
            Quaternion.Euler(
                0,
                0,
                hourOffset
            );

        minuteHand.localRotation =
            minuteStartRotation *
            Quaternion.Euler(
                0,
                0,
                minuteOffset
            );

        Debug.Log(
            $"RELOJ -> {currentHour}:{currentMinute:00}"
        );
    }

    void ClosePuzzle()
    {
        IsOpen = false;

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