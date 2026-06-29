using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class mapcon : MonoBehaviour
{
    public Sprite statOpened;
    public Sprite statClosed;

    public static string selectedStatus;
    public static string selectedDoor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( (gameObject.name == selectedDoor))
        {
            if (selectedStatus == "unlock")
            {
                GetComponent<Image>().sprite = statOpened;
            }
            if (selectedStatus == "locked")
            {
                GetComponent<Image>().sprite = statClosed;
            }
        }


    }
}
