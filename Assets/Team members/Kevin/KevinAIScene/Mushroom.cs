using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using UnityEngine;

namespace Kevin
{
    public class Mushroom : Food, IEdible, IEntity
    {
        public void Start()
        {
            healthValue = 15f; 
        }

      
    } 
}

