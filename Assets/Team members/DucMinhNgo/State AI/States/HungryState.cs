using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;

public class HungryState : Basestate
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SimpleAI>().Foodfinding += Findfood;
        Findfood();
    }

    // Update is called once per frame
    void Findfood()
    {
        GetComponent<Renderer>().material.color = Color.magenta;
    }
}
