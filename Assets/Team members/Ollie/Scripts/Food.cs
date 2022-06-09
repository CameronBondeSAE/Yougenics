using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class Food : NPCBehaviour, iNPC, iFood
    {
        private void Start()
        {
            SetNpcType();
        }
    }
}
