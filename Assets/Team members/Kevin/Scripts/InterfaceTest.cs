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
    }

    public interface IEntity
    {
        
    }

    public interface IEdible
    {
        
    }
}

