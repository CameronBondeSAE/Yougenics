using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Luke
{
    public class AStarNode : NodeBase
    {
        public bool isBlocked;
        public bool isClosed;
        
        public AStarNode parent;
        private int _gCost;
        private int _hCost;
        public int fCost;

        public int gCostWeight = 1;
        public int hCostWeight = 1;

        public int GCost
        {
	        get => _gCost;
	        set
	        {
		        _gCost = value;
		        fCost = gCostWeight*_gCost + hCostWeight*_hCost;
	        }
        }
        
        public int HCost
        {
            get => _hCost;
            set
            {
                _hCost = value;
                fCost = gCostWeight*_gCost + hCostWeight*_hCost;
            }
        }
    }
}