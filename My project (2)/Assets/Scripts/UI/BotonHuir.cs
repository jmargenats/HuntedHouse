using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonHuir : MonoBehaviour
{
    // Start is called before the first frame update
    public SceneFader sceneFader;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunAway()
    {
        GameManager.Instance.returningFromBattle = true;
        GameManager.Instance.ratDefeated = true;
        sceneFader.FadeAndLoadScene(GameManager.Instance.previousScene);
    }
}
