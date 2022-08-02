using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;

namespace Luke
{
	public class AStarUser : MonoBehaviour
	{
		public LukeAStar aStar;
		public bool[,] NodeClosedState;
		public Vector3 endLocation;
		public AStarNode CurrentNode;
		public AStarNode StartNode;
		public AStarNode EndNode;
		public List<AStarNode> _openNodes = new();
		public List<AStarNode> path = new();
		public bool breaker;

		void Start()
		{
			aStar = LukeAStar.Instance;
			CopyGrid();
		}

		public void CopyGrid()
		{
			NodeClosedState = new bool[aStar.Nodes.GetLength(0),aStar.Nodes.GetLength(1)];
		}

		public void AStarAlgorithmFast()
        {
	        CurrentNode = StartNode;
	        CurrentNode.GCost = Mathf.RoundToInt(1000*Vector3.Distance(CurrentNode.worldPosition, endLocation));
	        CurrentNode.HCost = 0;
	        _openNodes.Add(CurrentNode);
	        CheckNeighbours();
	        CurrentNode = _openNodes[OpenNodesComparison()];
	        if (CurrentNode != EndNode) AStarLoopFast();
	        else CreatePath();
        }

		private void AStarLoopFast()
		{
			while (CurrentNode != EndNode)
			{
				Debug.Log(CurrentNode.indices[0]+" "+CurrentNode.indices[1]);
		        CheckNeighbours(CurrentNode);
		        if (_openNodes.Count > 0) CurrentNode = _openNodes[OpenNodesComparison()];
	        }
			if(CurrentNode == EndNode) CreatePath();
        }

        private void CreatePath()
        {
	        path.Clear();
	        while (CurrentNode != StartNode && CurrentNode.parent != null)
	        {
		        path.Add(CurrentNode);
		        CurrentNode = CurrentNode.parent;
	        }
        }

        //Case: First Node
        private void CheckNeighbours()
        {
            int i = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
	                int indexX = CurrentNode.indices[0] - 1 + x;
	                int indexY = CurrentNode.indices[1] - 1 + y;
	                if (indexX < 0 || indexX >= aStar.Nodes.GetLength(0)) continue;
	                if (indexY < 0 || indexY >= aStar.Nodes.GetLength(1)) continue;
	                AStarNode neighbour = aStar.Nodes[indexX, indexY];
                    if (neighbour.isBlocked || NodeClosedState[neighbour.indices[0], neighbour.indices[1]] || _openNodes.Contains(neighbour)) continue;
                    i++;
                    _openNodes.Add(neighbour);
                    neighbour.GCost = Mathf.RoundToInt(1000*Vector3.Distance(neighbour.worldPosition, endLocation));
                    neighbour.parent = CurrentNode;
                    neighbour.HCost =
                            Mathf.RoundToInt(1000*Vector3.Distance(neighbour.worldPosition, CurrentNode.worldPosition) +
                                             CurrentNode.HCost);
                }
            }
            NodeClosedState[CurrentNode.indices[0], CurrentNode.indices[1]] = true;
            _openNodes.Remove(CurrentNode);
        }
        
        private void CheckNeighbours(AStarNode node)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
	                int indexX = node.indices[0] - 1 + x;
	                int indexY = node.indices[1] - 1 + y;
	                if (indexX < 0 || indexX >= aStar.Nodes.GetLength(0)) continue;
	                if (indexY < 0 || indexY >= aStar.Nodes.GetLength(1)) continue;
	                AStarNode neighbour = aStar.Nodes[indexX, indexY];
                    if (neighbour.isBlocked || NodeClosedState[neighbour.indices[0], neighbour.indices[1]] || _openNodes.Contains(neighbour)) continue;
                    _openNodes.Add(neighbour);
                    neighbour.GCost = Mathf.RoundToInt(1000*Vector3.Distance(neighbour.worldPosition, endLocation));
                    if (neighbour.parent != null && neighbour.HCost < node.parent.HCost)
                    {
                        node.parent = neighbour;
                        node.HCost = Mathf.RoundToInt(1000*Vector3.Distance(neighbour.worldPosition, node.worldPosition) +
                                                      neighbour.HCost);
                    }
                    else
                    {
                        neighbour.parent = node;
                        neighbour.HCost =
                            Mathf.RoundToInt(1000*Vector3.Distance(neighbour.worldPosition, node.worldPosition) +
                                             node.HCost);
                    }
                }
            }
            NodeClosedState[node.indices[0], node.indices[1]] = true;
            _openNodes.Remove(node);
        }

        private int OpenNodesComparison()
        {
	        int lowestFCostIndex = 0;
	        int lowestGCostIndex = 0;
	        int lowestFCost = _openNodes[lowestFCostIndex].fCost;
	        int lowestGCost = _openNodes[lowestFCostIndex].GCost;
	        for (int i=0; i < _openNodes.Count; i++)
	        {
		        if (_openNodes[i].fCost < lowestFCost)
		        {
			        lowestFCost = _openNodes[i].fCost;
			        lowestFCostIndex = i;
			        lowestGCost = _openNodes[i].GCost;
			        lowestGCostIndex = i;
		        }
		        else if (_openNodes[i].fCost == lowestFCost && _openNodes[i].GCost < lowestGCost)
		        {
			        lowestGCost = _openNodes[i].GCost;
			        lowestGCostIndex = i;
		        }
	        }

	        if (lowestFCostIndex == lowestGCostIndex) return lowestFCostIndex;
	        return lowestGCostIndex;
        }
        
        public void ResetNodes(Vector3 startPosition, Vector3 targetPosition)
        {
	        breaker = true;
	        _openNodes.Clear();
	        NodeClosedState = new bool[aStar.Nodes.GetLength(0),aStar.Nodes.GetLength(1)];
	        for (int i = 1; i < NodeClosedState.GetLength(0); i++)
	        {
		        for (int j = 0; j < NodeClosedState.GetLength(1); j++)
		        {
			        NodeClosedState[i, j] = false;
		        }
	        }
	        int[] index = aStar.ConvertIndexAndPosition(startPosition);
	        StartNode = aStar.Nodes[index[0], index[1]];
	        CurrentNode = StartNode;
	        index = aStar.ConvertIndexAndPosition(targetPosition); 
	        EndNode = aStar.Nodes[index[0], index[1]];
        }
	}
}