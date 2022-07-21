using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minh
{
    public class Basestate : MonoBehaviour
    {
        // Start is called before the first frame update
        public Statemanager stateManager;

        public void Awake()
        {
            stateManager = GetComponent<Minh.Statemanager>();

        }
    }
}

