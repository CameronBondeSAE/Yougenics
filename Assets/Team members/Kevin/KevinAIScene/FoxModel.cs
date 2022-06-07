using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class FoxModel : CreatureBase
    {
        public void Awake()
        {
            healthPoints = 100f;
            energyPoints = 100f;
            insightRange = 6f;
            attackRange = 2f;
        }
    }
}

