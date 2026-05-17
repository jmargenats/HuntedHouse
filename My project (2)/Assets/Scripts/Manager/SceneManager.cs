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


}