using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public SceneFader sceneFader;

    private bool paused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;

        paused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;

        paused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        EventSystem.current.SetSelectedGameObject(null);
        
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;

        sceneFader.FadeAndLoadScene("MainMenu");
    }

    public void SaveGame()
    {
        Debug.Log("Juego guardado");
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        GameManager.Instance.playerPosition =
            player.transform.position;

        GameManager.Instance.SaveGame();

        Debug.Log("Juego guardado despues ");
    }
}