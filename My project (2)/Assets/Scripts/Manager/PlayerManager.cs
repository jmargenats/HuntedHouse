using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.playerPosition != Vector3.zero)
            {
                transform.position = GameManager.Instance.playerPosition;
            }
        }
    }
}