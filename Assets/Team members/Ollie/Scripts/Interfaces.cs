using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public interface iNPC
    {
        void SetNpcType();
    }

    public interface iFood
    {
        
    }

    public interface iPredator
    {
        
    }

    public interface iCritter
    {
        
    }

    public interface iHeapItem<T> : IComparable<T>
    {
        int heapIndex
        {
            get;
            set;
        }
    }

    public interface iPathable
    {
        void GeneratePath(WaterNode node);
        void ClearPath();
    }
}
