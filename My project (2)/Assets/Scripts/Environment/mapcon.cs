using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapcon : MonoBehaviour
{
    public Sprite statOpened;
    public Sprite statClosed;

    public static Dictionary<string, string> doorStates =
        new Dictionary<string, string>();

    private Image img;

    void Start()
    {
        img = GetComponent<Image>();

        Color c = img.color;
        c.a = 0f;
        img.color = c;
    }

    void Update()
    {
        if (!doorStates.ContainsKey(gameObject.name))
            return;

        string status = doorStates[gameObject.name];

        Color c = img.color;
        c.a = 1f;
        img.color = c;

        if (status == "unlock")
            img.sprite = statOpened;
        else if (status == "locked")
            img.sprite = statClosed;
    }
}