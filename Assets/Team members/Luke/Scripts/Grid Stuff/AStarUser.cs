using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Luke
{
	public class AStarUser : MonoBehaviour
	{
		public LukeAStar aStar;
		public bool[,] nodeClosedState;
		public Vector3 endLocation;
		public AStarNode currentNode;
		public AStarNode startNode;
        public AStarNode endNode;
		public List<AStarNode> openNodes = new();
		public List<AStarNode> path = new();
		public bool breaker;

		void Start()
		{
			aStar = LukeAStar.Instance;
			CopyGrid();
		}

		public void CopyGrid()
		{
			nodeClosedState = new bool[aStar.Nodes.GetLength(0),aStar.Nodes.GetLength(1)];
		}

		public void AStarAlgorithmFast()
        {
	        currentNode = startNode;
	        currentNode.GCost = Mathf.RoundToInt(1000*Vector3.Distance(currentNode.worldPosition, endLocation));
	        currentNode.HCost = 0;
	        openNodes.Add(currentNode);
	        CheckNeighbours();
	        currentNode = openNodes[OpenNodesComparison()];
	        if (currentNode != endNode) AStarLoopFast();
	        else CreatePath();
        }

		private void AStarLoopFast()
		{
			while (currentNode != endNode)
			{
				Debug.Log(currentNode.indices[0]+" "+currentNode.indices[1]);
		        CheckNeighbours(currentNode);
		        if (openNodes.Count > 0) currentNode = openNodes[OpenNodesComparison()];
	        }
			if(currentNode == endNode) CreatePath();
        }

        private void CreatePath()
        {
	        path.Clear();
	        while (currentNode != startNode && currentNode.parent != null)
	        {
		        path.Add(currentNode);
		        currentNode = currentNode.parent;
	        }
        }

        //Case: First Node
        private void CheckNeighbours()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
	                int indexX = currentNode.indices[0] - 1 + x;
	                int indexY = currentNode.indices[1] - 1 + y;
	                if (indexX < 0 || indexX >= aStar.Nodes.GetLength(0)) continue;
	                if (indexY < 0 || indexY >= aStar.Nodes.GetLength(1)) continue;
	                AStarNode neighbour = aStar.Nodes[indexX, indexY];
                    if (neighbour.isBlocked || nodeClosedState[neighbour.indices[0], neighbour.indices[1]] || openNodes.Contains(neighbour)) continue;
                    openNodes.Add(neighbour);
                    neighbour.GCost = Mathf.RoundToInt(1000*Vector3.Distance(neighbour.worldPosition, endLocation));
                    neighbour.parent = currentNode;
                    neighbour.HCost =
                            Mathf.RoundToInt(1000*Vector3.Distance(neighbour.worldPosition, currentNode.worldPosition) +
                                             currentNode.HCost);
                }
            }
            nodeClosedState[currentNode.indices[0], currentNode.indices[1]] = true;
            openNodes.Remove(currentNode);
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
                    if (neighbour.isBlocked || nodeClosedState[neighbour.indices[0], neighbour.indices[1]] || openNodes.Contains(neighbour)) continue;
                    openNodes.Add(neighbour);
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
            nodeClosedState[node.indices[0], node.indices[1]] = true;
            openNodes.Remove(node);
        }

        private int OpenNodesComparison()
        {
	        int lowestFCostIndex = 0;
	        int lowestGCostIndex = 0;
	        int lowestFCost = openNodes[lowestFCostIndex].fCost;
	        int lowestGCost = openNodes[lowestFCostIndex].GCost;
	        for (int i=0; i < openNodes.Count; i++)
	        {
		        if (openNodes[i].fCost < lowestFCost)
		        {
			        lowestFCost = openNodes[i].fCost;
			        lowestFCostIndex = i;
			        lowestGCost = openNodes[i].GCost;
			        lowestGCostIndex = i;
		        }
		        else if (openNodes[i].fCost == lowestFCost && openNodes[i].GCost < lowestGCost)
		        {
			        lowestGCost = openNodes[i].GCost;
			        lowestGCostIndex = i;
		        }
	        }

	        if (lowestFCostIndex == lowestGCostIndex) return lowestFCostIndex;
	        return lowestGCostIndex;
        }
        
        public void ResetNodes(Vector3 startPosition, Vector3 targetPosition)
        {
	        breaker = true;
	        openNodes.Clear();
	        nodeClosedState = new bool[aStar.Nodes.GetLength(0),aStar.Nodes.GetLength(1)];
	        for (int i = 1; i < nodeClosedState.GetLength(0); i++)
	        {
		        for (int j = 0; j < nodeClosedState.GetLength(1); j++)
		        {
			        nodeClosedState[i, j] = false;
		        }
	        }
	        int[] index = aStar.ConvertIndexAndPosition(startPosition);
	        startNode = aStar.Nodes[index[0], index[1]];
	        currentNode = startNode;
	        index = aStar.ConvertIndexAndPosition(targetPosition); 
	        endNode = aStar.Nodes[index[0], index[1]];
        }
	}
}