using System;
using System.Collections;
using System.Collections.Generic;
using Ollie;
using UnityEngine;

namespace Ollie
{
    public class Predator : NPCBehaviour, iNPC, iPredator
    {
        public override void Start()
        {
            base.Start();

            SetNpcType();
        }
        
        public void SetNpcType()
        {
            npcType = NpcType.Predator;
        }
    }
}
