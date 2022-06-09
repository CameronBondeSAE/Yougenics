using System;
using System.Collections;
using System.Collections.Generic;
using Ollie;
using UnityEngine;

namespace Ollie
{
    public class Predator : NPCBehaviour, iNPC, iPredator
    {
        private void Start()
        {
            SetNpcType();
        }
    }
}
