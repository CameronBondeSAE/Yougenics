using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minh{ 
public class Gamemanager : MonoBehaviour
{
    public enum  States
    {
        Attacking,
        Findingfood,
        Sleeping
    }

    public GameObject min;

    public States currentState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (currentState == States.Attacking)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (currentState == States.Findingfood)
        {
            GetComponent<Renderer>().material.color = Color.green;
            
        }
        else if (currentState == States.Sleeping )
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
        
             
        
    }
}}
