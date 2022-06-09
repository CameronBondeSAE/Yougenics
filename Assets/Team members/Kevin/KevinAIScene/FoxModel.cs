using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class FoxModel : CreatureBase
    {
        public Gender gender;
        public void Awake()
        {
            gender = (Gender) Random.Range(0, 2);
            healthPoints = 100f;
            energyPoints = 100f;
            insightRange = 10f;
            attackRange = 1f;
            matingRange = 1f;
        }
    }
}

