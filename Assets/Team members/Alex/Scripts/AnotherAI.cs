using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StateBase stateBase = new StateBase();

        stateBase.EatingTest();

        Eating eating = new Eating();
        eating.EatingTest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
