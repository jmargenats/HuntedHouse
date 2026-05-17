using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool returningFromBattle = false;
    public bool ratDefeated = false;

    [Header("Player Data")]
    public Vector3 playerPosition;

    [Header("Scene Data")]
    public string previousScene;

    [Header("Inventario")]
    public List<string> collectedItems =
    new List<string>();
    public bool ignoreRatTriggerOnce = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(string itemType)
    {
        if (!collectedItems.Contains(itemType))
        {
            collectedItems.Add(itemType);
        }
    }

    public bool HasItem(string itemType)
    {
        return collectedItems.Contains(itemType);
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString(
            "Scene",
            SceneManager.GetActiveScene().name
        );

        PlayerPrefs.SetFloat("PlayerX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerZ", playerPosition.z);

        PlayerPrefs.SetInt(
            "RatDefeated",
            ratDefeated ? 1 : 0
        );

        PlayerPrefs.SetString(
            "Items",
            string.Join(",", collectedItems)
        );

        PlayerPrefs.Save();

        Debug.Log("Juego guardado");
    }

    public void LoadGame()
    {
        string sceneName =
            PlayerPrefs.GetString("Scene", "");

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("No hay save");
            return;
        }

        playerPosition = new Vector3(
            PlayerPrefs.GetFloat("PlayerX"),
            PlayerPrefs.GetFloat("PlayerY"),
            PlayerPrefs.GetFloat("PlayerZ")
        );

        ratDefeated =
            PlayerPrefs.GetInt("RatDefeated") == 1;

        collectedItems.Clear();

        string items =
            PlayerPrefs.GetString("Items", "");

        if (!string.IsNullOrEmpty(items))
        {
            collectedItems.AddRange(items.Split(','));
        }

        returningFromBattle = true;

        SceneManager.LoadScene(sceneName);
    }

    public void ResetGame()
    {
        returningFromBattle = false;
        ratDefeated = false;

        playerPosition = Vector3.zero;

        previousScene = "";

        collectedItems.Clear();

        ignoreRatTriggerOnce = false;

        PlayerPrefs.DeleteAll();

        Debug.Log("Juego reseteado");
    }
}