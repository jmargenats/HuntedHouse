using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorcon : MonoBehaviour
{
    // Start is called before the first frame update

    public string doorstatus;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        mapcon.selectedStatus = doorstatus;
        mapcon.selectedDoor = gameObject.name;
    }
}
