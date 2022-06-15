using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public interface INpc
    {
        public void Patrol();
        public void Chase();
        public void Attack();
        
        public void Sleep();

        public void Mate();
    }
    
    public interface IEntity
    {
        
    }

    public interface IEdible
    {
        
    }

    /*public enum Gender
    {
        Female,
        Male,
        NonBinary
    }*/
}

