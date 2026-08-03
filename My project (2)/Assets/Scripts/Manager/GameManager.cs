using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Map")]
    public bool hasMap = false;

    [Header("Ending")]
    public bool helpedTomasEscape = false;
    public bool playerDied = false;

    [Header("Tomas")]
    public int tomasConversationStage = 0;
    public bool bearUnlocked = false;
    public bool bearDelivered = false;

    [Header("Battle")]
    public bool returningFromBattle = false;
    public bool ignoreRatTriggerOnce = false;

    [Header("Player Data")]
    public Vector3 playerPosition;

    [Header("Doors")]
    public bool screwdriverLockRemoved = false;
    public bool screwdriverUsed = false;
    [Header("Scene Data")]
    public string previousScene;

    [Header("Inventario")]
    public List<string> collectedItems =
        new List<string>();

    [Header("Enemy Data")]
    public string currentEnemyID;

    public bool ratDefeated = false;

    public List<string> defeatedEnemies =
        new List<string>();

    [Header("Radio Puzzle")]
    public bool radioDiscovered = false;
    public bool penCollected = false;
    public bool cassetteUnlocked = false;
    public bool cassetteCollected = false;
    public bool radioPlayed = false;
    public bool cassetteNeedTool = false;
    public bool cassetteDiscovered = false;

    [Header("Tutorials")]
    public bool battleTutorialCompleted = false;

    public bool hasPendingSceneSpawn;
    public Vector3 pendingSpawnPosition;
    public Vector3 pendingSpawnRotation;
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

    // =========================
    // INVENTARIO
    // =========================

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

    // =========================
    // ENEMIGOS
    // =========================

    public bool IsEnemyDefeated(string enemyID)
    {
        return defeatedEnemies.Contains(enemyID);
    }
    public void DefeatEnemy(string enemyID)
    {
        if (!defeatedEnemies.Contains(enemyID))
        {
            defeatedEnemies.Add(enemyID);
        }
    }

    // =========================
    // SAVE
    // =========================

    public void SaveGame()
    {
        PlayerPrefs.SetInt(
            "ScrewdriverUsed",
            screwdriverUsed ? 1 : 0
        );
        PlayerPrefs.SetInt(
            "ScrewdriverLockRemoved",
            screwdriverLockRemoved ? 1 : 0
        );
        PlayerPrefs.SetInt(
            "HelpedTomasEscape",
            helpedTomasEscape ? 1 : 0
        );
        PlayerPrefs.SetInt(
            "TomasConversationStage",
            tomasConversationStage
        );

        PlayerPrefs.SetInt(
            "BearUnlocked",
            bearUnlocked ? 1 : 0
        );

        PlayerPrefs.SetInt(
            "BearDelivered",
            bearDelivered ? 1 : 0
        );
        PlayerPrefs.SetString(
            "Scene",
            SceneManager.GetActiveScene().name
        );

        PlayerPrefs.SetInt(
            "HasMap",
            hasMap ? 1 : 0
        );

        PlayerPrefs.SetFloat(
            "PlayerX",
            playerPosition.x
        );

        PlayerPrefs.SetFloat(
            "PlayerY",
            playerPosition.y
        );

        PlayerPrefs.SetFloat(
            "PlayerZ",
            playerPosition.z
        );

        PlayerPrefs.SetInt(
            "RatDefeated",
            ratDefeated ? 1 : 0
        );

        PlayerPrefs.SetString(
            "Items",
            string.Join(",", collectedItems)
        );

        PlayerPrefs.SetString(
            "DefeatedEnemies",
            string.Join(",", defeatedEnemies)
        );
        //prefab main scene

        PlayerPrefs.SetInt("RadioDiscovered", radioDiscovered ? 1 : 0);
        PlayerPrefs.SetInt("PenCollected", penCollected ? 1 : 0);
        PlayerPrefs.SetInt("CassetteUnlocked", cassetteUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("CassetteCollected", cassetteCollected ? 1 : 0);
        PlayerPrefs.SetInt("RadioPlayed", radioPlayed ? 1 : 0);
        PlayerPrefs.SetInt("CassetteNeedTool", cassetteNeedTool ? 1 : 0);
        PlayerPrefs.SetInt("CassetteDiscovered", cassetteDiscovered ? 1 : 0);

        PlayerPrefs.Save();

        Debug.Log("Juego guardado");
    }

    // =========================
    // LOAD
    // =========================

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
        screwdriverLockRemoved =
    PlayerPrefs.GetInt(
        "ScrewdriverLockRemoved",
        0
    ) == 1;
        ratDefeated =
            PlayerPrefs.GetInt("RatDefeated") == 1;
        hasMap =
            PlayerPrefs.GetInt("HasMap") == 1;
        helpedTomasEscape =
            PlayerPrefs.GetInt("HelpedTomasEscape", 0) == 1;
        tomasConversationStage =
            PlayerPrefs.GetInt(
        "TomasConversationStage",
                0
            );
        screwdriverUsed =
    PlayerPrefs.GetInt(
        "ScrewdriverUsed",
        0
    ) == 1;
        bearUnlocked =
            PlayerPrefs.GetInt(
                "BearUnlocked",
                0
            ) == 1;

        bearDelivered =
            PlayerPrefs.GetInt(
                "BearDelivered",
                0
            ) == 1;
        // ITEMS

        collectedItems.Clear();

        string items =
            PlayerPrefs.GetString("Items", "");

        if (!string.IsNullOrEmpty(items))
        {
            collectedItems.AddRange(
                items.Split(',')
            );
        }

        // ENEMIGOS DERROTADOS

        defeatedEnemies.Clear();

        string defeated =
            PlayerPrefs.GetString(
                "DefeatedEnemies",
                ""
            );

        if (!string.IsNullOrEmpty(defeated))
        {
            defeatedEnemies.AddRange(
                defeated.Split(',')
            );
        }

        //objetos de la main scene:
        radioDiscovered = PlayerPrefs.GetInt("RadioDiscovered", 0) == 1;
        penCollected = PlayerPrefs.GetInt("PenCollected", 0) == 1;
        cassetteUnlocked = PlayerPrefs.GetInt("CassetteUnlocked", 0) == 1;
        cassetteCollected = PlayerPrefs.GetInt("CassetteCollected", 0) == 1;
        radioPlayed = PlayerPrefs.GetInt("RadioPlayed", 0) == 1;
        cassetteNeedTool = PlayerPrefs.GetInt("CassetteNeedTool", 0) == 1;
        cassetteDiscovered = PlayerPrefs.GetInt("CassetteDiscovered", 0) == 1;

        returningFromBattle = true;

        SceneManager.LoadScene(sceneName);
    }

    // =========================
    // RESET
    // =========================

    public void ResetGame()
    {
        returningFromBattle = false;
        helpedTomasEscape = false;
        tomasConversationStage = 0;
        bearUnlocked = false;
        bearDelivered = false;
        ratDefeated = false;

        playerPosition =
            Vector3.zero;

        previousScene = "";
        screwdriverLockRemoved = false;
        currentEnemyID = "";

        collectedItems.Clear();

        defeatedEnemies.Clear();

        ignoreRatTriggerOnce = false;
        screwdriverUsed = false;
        radioDiscovered = false;
        penCollected = false;
        cassetteUnlocked = false;
        cassetteCollected = false;
        radioPlayed = false;
        cassetteNeedTool = false;
        cassetteDiscovered = false;
        battleTutorialCompleted = false;

        hasMap = false;

        PlayerPrefs.DeleteAll();

        Debug.Log("Juego reseteado");
    }
}