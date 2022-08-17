using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ollie
{
    public class Food : NPCBehaviour, iNPC, iFood, IEdible
    {
        public override void Start()
        {
            base.Start();
            

            SetNpcType();
        }
        
        public void SetNpcType()
        {
            npcType = NpcType.Food;
        }

        public float GetEnergyAmount()
        {
            throw new NotImplementedException();
        }

        public float EatMe(float energyRemoved)
        {
            throw new NotImplementedException();
        }
    }
}
