using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class AnotherAI : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Alex.StateBase stateBase = new Alex.StateBase();

            stateBase.EatingTest();

            Eating eating = new Eating();
            eating.EatingTest();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
