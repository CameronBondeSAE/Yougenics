using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
    {
        public class NodeClass
        {
            public Vector3 worldPosition; 
            public bool walkable;

            public NodeClass(Vector3 _worldPosition, bool _walkable)
            {
                worldPosition = _worldPosition;
                walkable = _walkable;
            }
        }
    
    }
