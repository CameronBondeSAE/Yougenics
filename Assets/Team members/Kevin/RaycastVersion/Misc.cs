using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class Misc : MonoBehaviour
    {
        public interface ICritter
        {
            public void Patrol();
            public void Hunting();
            public void Eating();
            public void Sleeping();
            public void Full();
            public void Mating();

        }
        public interface IEntity
        {
        
        }

        public interface IEdible
        {
            
        }

        public enum FoodChain
        {
            Predator,
            Prey,
            Neutral
        }

        public enum Gender
        {
            Female,
            Male
        }
    }
}

