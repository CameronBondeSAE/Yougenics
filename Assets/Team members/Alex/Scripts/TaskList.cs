using System.Collections;
using System.Collections.Generic;
using Alex;
using ParadoxNotion;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskList : MonoBehaviour
{


    public bool toggle;
    public Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        ToggleTest();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GetComponent<Player>().energy.energyAmount <= 0)
        {
            toggle = true;
        }
        else
        {
            toggle = false;
        }
        
        ToggleTest();
    }

    public void ToggleTest()
    {
        if (GetComponent<Player>().energy.energyAmount <= 0)
        {
            toggle = true;
        }
        else
        {
            toggle = false;
        }
    }
    
}
