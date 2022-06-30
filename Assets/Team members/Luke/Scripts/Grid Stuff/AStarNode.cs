using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
    public class AStarNode : NodeBase
    {
        public bool isBlocked;
        public bool isClosed;
        
        public AStarNode parent;
        public float distanceToEnd;
        private float cumulativePathDistance;
        public float fCost;

        public float CumulativePathDistance
        {
            get => cumulativePathDistance;
            set
            {
                cumulativePathDistance = value;
                fCost = distanceToEnd + cumulativePathDistance;
            }
        }
    }
}