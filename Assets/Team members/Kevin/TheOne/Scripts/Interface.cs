using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kev
{
    public class Interface : MonoBehaviour
    {
        public interface ICritter
        {
            public void Patrol();
            public void Chase();
            public void BeginEat(Collider other);
            public void BeginMate();

            public void Sleepy();
            public void Death();
        }

        public interface IEntity
        {
            
        }

        public interface IEdible
        {
            
        }
        
        public enum Gender
        {
            Female,
            Male
        }

        public enum Foodchain
        {
            Predator,
            Prey,
            Neutral
        }
    }
}