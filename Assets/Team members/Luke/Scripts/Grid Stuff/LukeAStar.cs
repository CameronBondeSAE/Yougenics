using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Luke
{
    public class LukeAStar : MonoBehaviour
    {
	    public static LukeAStar Instance;
        
	    
        
        #region MyRegion

        public bool slowMode;
        
        public AStarNode[,] Nodes;

        [SerializeField] private Vector2Int numberOfTiles;
        [SerializeField] private float gridTileHeight;
        [SerializeField] private Vector2 gridSize;
        [SerializeField] private Vector3 gridOrigin;

        [SerializeField] private int gCostWeight = 1;
        [SerializeField] private int hCostWeight = 1;

        private Vector3 _gridTileSize;

        public AStarNode CurrentNode;
        public AStarNode StartNode;
        public AStarNode EndNode;
        public Vector3 startLocation;
        public Vector3 endLocation;
        private List<AStarNode> _openNodes = new();
        public List<AStarNode> path = new();

        public Coroutine coroutineInstance = null; //For stopping and resetting algorithm.

        public Action FinishedFillingGridEvent;
        
        #endregion

        #region Methods
		
		private void FillGrid()
        {
            _gridTileSize = new Vector3(gridSize.x/numberOfTiles.x, gridTileHeight, gridSize.y/numberOfTiles.y);
            
			Nodes = new AStarNode[numberOfTiles.x, numberOfTiles.y];
			for (int x = 0; x < numberOfTiles.x; x++)
			{
				for (int y = 0; y < numberOfTiles.y; y++)
				{
                    Nodes[x, y] = new AStarNode
					{
						worldPosition = ConvertIndexAndPosition(new []{x,y}),
						gCostWeight = gCostWeight,
						hCostWeight = hCostWeight,
						indices = new []{x,y}
					};
                    
                    if (Physics.OverlapBox(Nodes[x,y].worldPosition, _gridTileSize*0.5f, 
                            Quaternion.identity, 128).Length != 0)
                    {
                        Nodes[x, y].isBlocked = true;
                    }
				}
			}
			IntroduceNeighbours();
			FinishedFillingGridEvent?.Invoke();
        }
		
		private void IntroduceNeighbours()
		{
			for (int x = 0; x < numberOfTiles.x; x++)
			{
				for (int y = 0; y < numberOfTiles.y; y++)
				{
					if (x > 0) Nodes[x-1, y].neighbours[2,1] = Nodes[x, y];
					if (y > 0) Nodes[x, y-1].neighbours[1,2] = Nodes[x, y];
					if (x < numberOfTiles.x-1) Nodes[x+1, y].neighbours[0,1] = Nodes[x, y];
					if (y < numberOfTiles.y-1) Nodes[x, y+1].neighbours[1,0] = Nodes[x, y];
					if (x > 0 && y > 0) Nodes[x-1, y-1].neighbours[2,2] = Nodes[x, y];
					if (x > 0 && y < numberOfTiles.y-1) Nodes[x-1, y+1].neighbours[2,0] = Nodes[x, y];
					if (x < numberOfTiles.x-1 && y > 0) Nodes[x+1, y-1].neighbours[0,2] = Nodes[x, y];
					if (x < numberOfTiles.x-1 && y < numberOfTiles.y-1) Nodes[x+1, y+1].neighbours[0,0] = Nodes[x, y];
				}
			}
		}

        public IEnumerator AStarAlgorithm()
        {
	        CurrentNode = StartNode;

            CurrentNode.GCost = Mathf.RoundToInt(1000*Vector3.Distance(CurrentNode.worldPosition, endLocation));
            CurrentNode.HCost = 0;
            _openNodes.Add(CurrentNode);

            CheckNeighbours();

            yield return new WaitForEndOfFrame();
            
            CurrentNode = _openNodes[OpenNodesComparison()];

            if (CurrentNode != EndNode && !slowMode) AStarLoopFast();
            else if (CurrentNode != EndNode) coroutineInstance = StartCoroutine(AStarLoop());
            else CreatePath();
        }
        
        public void AStarAlgorithmFast()
        {
	        CurrentNode = StartNode;
	        CurrentNode.GCost = Mathf.RoundToInt(1000*Vector3.Distance(CurrentNode.worldPosition, endLocation));
	        CurrentNode.HCost = 0;
	        _openNodes.Add(CurrentNode);

	        CheckNeighbours();
            
	        CurrentNode = _openNodes[OpenNodesComparison()];

	        if (CurrentNode != EndNode && !slowMode) AStarLoopFast();
	        else if (CurrentNode != EndNode) coroutineInstance = StartCoroutine(AStarLoop());
	        else CreatePath();
        }

        private IEnumerator AStarLoop()
        {
            CheckNeighbours(CurrentNode);
            
            yield return new WaitForEndOfFrame();
            
            CurrentNode = _openNodes[OpenNodesComparison()];

            if (CurrentNode != EndNode && !slowMode) AStarLoopFast();
            else if (CurrentNode != EndNode) coroutineInstance = StartCoroutine(AStarLoop());
            else CreatePath();
        }
        
        private void AStarLoopFast()
        {
	        CheckNeighbours(CurrentNode);
	        
	        if (_openNodes.Count > 0) CurrentNode = _openNodes[OpenNodesComparison()];

	        if (CurrentNode != EndNode && !slowMode) AStarLoopFast();
	        else if (CurrentNode != EndNode) coroutineInstance = StartCoroutine(AStarLoop());
	        else CreatePath();
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
                    AStarNode neighbour = (AStarNode)CurrentNode.neighbours[x, y];
                    if (neighbour == null) continue;
                    if (neighbour.isBlocked || neighbour.isClosed || _openNodes.Contains(neighbour)) continue;
                    i++;
                    _openNodes.Add(neighbour);
                    neighbour.GCost = Mathf.RoundToInt(1000*Vector3.Distance(neighbour.worldPosition, endLocation));
                    neighbour.parent = CurrentNode;
                    neighbour.HCost =
                            Mathf.RoundToInt(1000*Vector3.Distance(neighbour.worldPosition, CurrentNode.worldPosition) +
                                             CurrentNode.HCost);
                }
            }
            CurrentNode.isClosed = true;
            _openNodes.Remove(CurrentNode);
        }
        
        private void CheckNeighbours(AStarNode node)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    AStarNode neighbour = (AStarNode)node.neighbours[x, y];
                    if (neighbour == null) continue;
                    if (neighbour.isBlocked || neighbour.isClosed || _openNodes.Contains(neighbour)) continue;
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
            node.isClosed = true;
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


        private Vector3 ConvertIndexAndPosition(int[] index)
        {
            return new Vector3(gridOrigin.x + index[0] * _gridTileSize.x, gridOrigin.y,
                gridOrigin.z + index[1] * _gridTileSize.z);
        }
        
        public int[] ConvertIndexAndPosition(Vector3 position)
        {
            return new [] {Mathf.RoundToInt((position.x-gridOrigin.x)/_gridTileSize.x), 
                Mathf.RoundToInt((position.z-gridOrigin.z)/_gridTileSize.z)};
        }
        
        public Vector3 RandomLocation()
        {
            Vector3 result = new (Random.Range(gridOrigin.x, gridOrigin.x+gridSize.x),gridOrigin.y,
                Random.Range(gridOrigin.z, gridOrigin.z+gridSize.y));
            int[] index = ConvertIndexAndPosition(result);

            if (Nodes[index[0], index[1]].isBlocked)
            {
                result = RandomLocation();
            }
            
            return result;
        }

        public void ResetNodes()
        {
	        if(coroutineInstance != null) StopCoroutine(coroutineInstance);
	        _openNodes.Clear();
	        foreach (AStarNode node in Nodes)
	        {
		        node.isClosed = false;
	        }
	        int[] index = ConvertIndexAndPosition(startLocation);
	        StartNode = Nodes[index[0], index[1]];
	        CurrentNode = StartNode;
	        index = ConvertIndexAndPosition(endLocation); 
	        EndNode = Nodes[index[0], index[1]];
        }

        public void ResetNodes(Vector3 startPosition, Vector3 targetPosition)
        {
	        if(coroutineInstance != null) StopCoroutine(coroutineInstance);
	        _openNodes.Clear();
	        foreach (AStarNode node in Nodes)
	        {
		        node.isClosed = false;
	        }
	        int[] index = ConvertIndexAndPosition(startPosition);
	        StartNode = Nodes[index[0], index[1]];
	        CurrentNode = StartNode;
	        index = ConvertIndexAndPosition(targetPosition); 
	        EndNode = Nodes[index[0], index[1]];
        }

        #endregion

        void Awake()
        {
	        Instance = this;
            FillGrid();
        }
        
        private void OnDrawGizmosSelected()
        {
            for (int x = 0; x < numberOfTiles.x; x++)
            {
                for (int y = 0; y < numberOfTiles.y; y++)
                {
                    if (Nodes == null) return;
                    if (Nodes[x, y] == CurrentNode)
                    {
                        Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
                        Gizmos.DrawCube(Nodes[x, y].worldPosition, _gridTileSize);
                    }
                    else if (Nodes[x, y] == EndNode)
                    {
	                    Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
	                    Gizmos.DrawCube(Nodes[x, y].worldPosition, _gridTileSize);
                    }
                    else if (Nodes[x, y].isBlocked)
                    {
                        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                        Gizmos.DrawCube(Nodes[x, y].worldPosition, _gridTileSize);
                    }
                    else if (Nodes[x, y].isClosed)
                    {
                        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                        Gizmos.DrawCube(Nodes[x, y].worldPosition, _gridTileSize);
                    }
                    else if (_openNodes.Contains(Nodes[x, y]))
                    {
                        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
                        Gizmos.DrawCube(Nodes[x, y].worldPosition, _gridTileSize);
                    }
                    else
                    {
                        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
                        Gizmos.DrawCube(Nodes[x, y].worldPosition, _gridTileSize);
                    }
                }
            }
        }
    }
}