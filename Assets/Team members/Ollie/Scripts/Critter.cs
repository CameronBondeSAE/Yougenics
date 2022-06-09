using System;
using System.Collections;
using System.Collections.Generic;
using Ollie;
using UnityEngine;

namespace Ollie
{
    public class Critter : NPCBehaviour, iNPC, iCritter
    {
        public Gender gender;
        
        private void Start()
        {
            SetGender();
            SetNpcType();
        }

        public enum Gender
        {
            Female,
            Male,
            NonBinary
        }

        private void SetGender()
        {
            var rng = UnityEngine.Random.Range(1, 101);
            if (rng <= 40)
            {
                gender = Gender.Female;
            }
            else if (rng <= 80 && rng >= 41)
            {
                gender = Gender.Male;
            }
            else
            {
                gender = Gender.NonBinary;
            }
        }
    }
}
