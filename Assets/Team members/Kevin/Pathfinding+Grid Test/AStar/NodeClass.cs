using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
    {
        [Serializable]
        public class NodeClass
        {
            public Vector3 worldPosition; 
            public bool isBlocked;
            public Vector2Int gridPosition;
    
            public int gCost;
            public int hCost;
            
            public NodeClass(Vector3 _worldPosition, bool _isBlocked, Vector2Int _gridPosition)
            {
                worldPosition = _worldPosition;
                isBlocked = _isBlocked;
                gridPosition = _gridPosition;
            }

            /*public NodeClass()
            {
                
            }*/
            public int fCost
            {
                get
                {
                    return gCost + hCost;
                }
            }
        }
    
    }
