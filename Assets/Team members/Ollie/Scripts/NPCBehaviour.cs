using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class NPCBehaviour : MonoBehaviour
    {
        public NpcType npcType;
        
        public virtual void Start()
        {
            
        }
        public enum NpcType
        {
            Food,
            Predator,
            Critter
        }
    }
}