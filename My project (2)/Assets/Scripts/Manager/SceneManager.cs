using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool returningFromBattle = false;
    public bool ratDefeated = false;

    [Header("Player Data")]
    public Vector3 playerPosition;

    [Header("Scene Data")]
    public string previousScene;

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
}