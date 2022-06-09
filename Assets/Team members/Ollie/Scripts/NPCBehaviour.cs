using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class NPCBehaviour : MonoBehaviour
    {
        public NpcType npcType;

        public enum NpcType
        {
            Food,
            Predator,
            Critter
        }

        public void SetNpcType()
        {
            var rng = UnityEngine.Random.Range(1, 101);
            if (rng <= 40)
            {
                npcType = NpcType.Food;
            }
            else if (rng >= 41 && rng <= 80)
            {
                npcType = NpcType.Critter;
            }
            else
            {
                npcType = NpcType.Predator;
            }
        }
    }
}