using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Luke
{
	public class AStarUser : MonoBehaviour
	{
		public LukeAStar aStar;
		private Vector2Int[,] _parents;
		public Vector3 endLocation;
		public Vector2Int currentNode;
		public Vector2Int startNode;
        public Vector2Int endNode;
		private List<Vector2Int> _openNodes = new();
		private List<Vector2Int> _closedNodes = new();
		private  int[,] _gCosts;
		private  int[,] _hCosts;
		private  int[,] _fCosts;
		public List<Vector2Int> path = new();
		public bool breaker;

		void Start()
		{
			aStar = LukeAStar.Instance;
			CopyGrid();
		}

		public void CopyGrid()
		{
			_parents = new Vector2Int[aStar.Nodes.GetLength(0),aStar.Nodes.GetLength(1)];
			_gCosts = new int[aStar.Nodes.GetLength(0),aStar.Nodes.GetLength(1)];
			_fCosts = new int[aStar.Nodes.GetLength(0),aStar.Nodes.GetLength(1)];
			_hCosts = new int[aStar.Nodes.GetLength(0),aStar.Nodes.GetLength(1)];
		}

		public void BeginAStarAlgorithm()
        {
	        currentNode = new Vector2Int(startNode.x, startNode.y);
	        _gCosts[currentNode.x, currentNode.y] = Mathf.RoundToInt(1000*Vector3.Distance(aStar.Nodes[currentNode.x,currentNode.y].worldPosition, endLocation));
	        _hCosts[currentNode.x, currentNode.y] = 0;
	        _fCosts[currentNode.x, currentNode.y] =
		        _gCosts[currentNode.x, currentNode.y] + _hCosts[currentNode.x, currentNode.y];
	        _openNodes.Add(currentNode);
	        CheckNeighbours();
	        currentNode = _openNodes[OpenNodesComparison()];
	        if (currentNode != endNode) AStarLoop();
	        else CreatePath();
        }

		private void AStarLoop()
		{
			while (currentNode != endNode)
			{
		        CheckNeighbours(currentNode);
		        if (_openNodes.Count > 0) break;
				currentNode = _openNodes[OpenNodesComparison()];
	        }
			if(currentNode == endNode) CreatePath();
        }

        private void CreatePath()
        {
	        while (_parents[currentNode.x, currentNode.y] != new Vector2Int(-1,-1))
	        {
		        path.Add(currentNode);
		        currentNode = _parents[currentNode.x, currentNode.y];
	        }
        }

        //Case: First Node
        private void CheckNeighbours()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
	                int indexX = currentNode.x - 1 + x;
	                int indexY = currentNode.y - 1 + y;
	                if (indexX < 0 || indexX >= aStar.Nodes.GetLength(0)) continue;
	                if (indexY < 0 || indexY >= aStar.Nodes.GetLength(1)) continue;
	                Vector2Int neighbour = new Vector2Int(indexX, indexY);
                    if (aStar.Nodes[neighbour.x,neighbour.y].isBlocked || _closedNodes.Contains(neighbour) || _openNodes.Contains(neighbour)) continue;
                    _openNodes.Add(neighbour);
                    _gCosts[indexX, indexY] = Mathf.RoundToInt(1000*Vector3.Distance(aStar.Nodes[indexX,indexY].worldPosition, endLocation));
                    _parents[indexX, indexY] = currentNode;
                    _hCosts[indexX, indexY] = Mathf.RoundToInt(1000*Vector3.Distance(aStar.Nodes[indexX,indexY].worldPosition,
	                                                               aStar.Nodes[currentNode.x,currentNode.y].worldPosition) +
                                                               _hCosts[currentNode.x,currentNode.y]);
                    _fCosts[indexX, indexY] = _gCosts[indexX, indexY] + _hCosts[indexX, indexY];
                }
            }
            _closedNodes.Add(currentNode);
            _openNodes.Remove(currentNode);
        }
        
        private void CheckNeighbours(Vector2Int node)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
	                int indexX = node.x - 1 + x;
	                int indexY = node.y - 1 + y;
	                if (indexX < 0 || indexX >= aStar.Nodes.GetLength(0)) continue;
	                if (indexY < 0 || indexY >= aStar.Nodes.GetLength(1)) continue;
	                Vector2Int neighbour = new Vector2Int(indexX, indexY);
	                if (aStar.Nodes[neighbour.x,neighbour.y].isBlocked || _closedNodes.Contains(neighbour) || _openNodes.Contains(neighbour)) continue;
                    _openNodes.Add(neighbour);
                    _gCosts[indexX, indexY] = Mathf.RoundToInt(1000*Vector3.Distance(aStar.Nodes[indexX,indexY].worldPosition, endLocation));
                    if (_parents[indexX,indexY] != new Vector2Int(-1,-1) && _hCosts[indexX, indexY] < _hCosts[_parents[node.x, node.y].x,_parents[node.x, node.y].y])
                    {
                        _parents[node.x, node.y] = neighbour;
                        _hCosts[node.x,node.y] = Mathf.RoundToInt(1000*Vector3.Distance(aStar.Nodes[indexX,indexY].worldPosition,
	                                                                  aStar.Nodes[node.x,node.y].worldPosition) + _hCosts[indexX, indexY]);
                        _fCosts[node.x, node.y] = _gCosts[node.x, node.y] + _hCosts[node.x, node.y];
                    }
                    else
                    {
                        _parents[indexX, indexY] = node;
                        _hCosts[indexX, indexY] =
                            Mathf.RoundToInt(1000*Vector3.Distance(aStar.Nodes[indexX,indexY].worldPosition, aStar.Nodes[node.x,node.y].worldPosition) +
                                             _hCosts[node.x,node.y]);
                        _fCosts[indexX, indexY] = _gCosts[indexX, indexY] + _hCosts[indexX, indexY];
                    }
                }
            }
            _closedNodes.Add(currentNode);
            _openNodes.Remove(node);
        }

        private int OpenNodesComparison()
        {
	        int lowestFCostIndex = 0;
	        int lowestGCostIndex = 0;
	        int lowestFCost = _fCosts[_openNodes[lowestFCostIndex].x, _openNodes[lowestFCostIndex].y];
	        int lowestGCost = _gCosts[_openNodes[lowestFCostIndex].x, _openNodes[lowestFCostIndex].y];
	        for (int i=0; i < _openNodes.Count; i++)
	        {
		        if (_fCosts[_openNodes[i].x, _openNodes[i].y] < lowestFCost)
		        {
			        lowestFCost = _fCosts[_openNodes[i].x, _openNodes[i].y];
			        lowestFCostIndex = i;
			        lowestGCost = _gCosts[_openNodes[i].x, _openNodes[i].y];
			        lowestGCostIndex = i;
		        }
		        else if (_fCosts[_openNodes[i].x, _openNodes[i].y] == lowestFCost && _gCosts[_openNodes[i].x, _openNodes[i].y] < lowestGCost)
		        {
			        lowestGCost = _gCosts[_openNodes[i].x, _openNodes[i].y];
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
            _closedNodes.Clear();
            CopyGrid();
            for (int i = 0; i < aStar.Nodes.GetLength(0); i++)
            {
	            for (int j = 0; j < aStar.Nodes.GetLength(1); j++)
	            {
		            _gCosts[i, j] = 0;
		            _hCosts[i, j] = 0;
		            _fCosts[i, j] = 0;
		            _parents[i,j] = new Vector2Int(-1,-1);
		            path.Clear();
	            }
            }
            
            int[] index = aStar.ConvertIndexAndPosition(startPosition);
	        startNode = new Vector2Int(index[0], index[1]);
	        currentNode = startNode;
	        index = aStar.ConvertIndexAndPosition(targetPosition); 
	        endNode = new Vector2Int(index[0], index[1]);
        }
	}
}