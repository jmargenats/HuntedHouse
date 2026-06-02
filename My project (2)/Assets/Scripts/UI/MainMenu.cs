
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneFader sceneFader;

    public void NewGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ResetGame();
        sceneFader.FadeAndLoadScene("Tutorial");
    }

    public void ContinueGame()
    {
        if (!PlayerPrefs.HasKey("Scene"))
        {
            Debug.Log("No hay save");
            return;
        }

        Time.timeScale = 1f;

        GameManager.Instance.LoadGame();
    }

    public void ExitGame()
    {
        Debug.Log("Salir del juego");

        Application.Quit();
    }
}

