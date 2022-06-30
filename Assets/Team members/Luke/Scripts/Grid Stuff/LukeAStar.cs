using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

namespace Luke
{
    public class LukeAStar : MonoBehaviour
    {
        
        #region MyRegion

        private AStarNode[,] _nodes;

        [SerializeField] private Vector2Int numberOfTiles;
        [SerializeField] private float gridTileHeight;
        [SerializeField] private Vector2 gridSize;
        [SerializeField] private Vector3 gridOrigin;

        private Vector3 _gridTileSize;

        public AStarNode currentNode;
        public Vector3 startLocation;
        public Vector3 endLocation;
        public List<AStarNode> openNodes = new();

        #endregion

        #region Methods
		
		public void FillGrid()
        {
            _gridTileSize = new Vector3(gridSize.x/numberOfTiles.x, gridTileHeight, gridSize.y/numberOfTiles.y);
            
			_nodes = new AStarNode[numberOfTiles.x, numberOfTiles.y];
			for (int x = 0; x < numberOfTiles.x; x++)
			{
				for (int y = 0; y < numberOfTiles.y; y++)
				{
                    _nodes[x, y] = new AStarNode
					{
						worldPosition = ConvertIndexAndPosition(new []{x,y})
                    };
                    
                    if (Physics.OverlapBox(_nodes[x,y].worldPosition, _gridTileSize*0.5f, 
                            Quaternion.identity, 251).Length != 0)
                    {
                        _nodes[x, y].isBlocked = true;
                    }
				}
			}
			IntroduceNeighbours();
		}
		
		private void IntroduceNeighbours()
		{
			for (int x = 0; x < numberOfTiles.x; x++)
			{
				for (int y = 0; y < numberOfTiles.y; y++)
				{
					if (x > 0) _nodes[x-1, y].neighbours[2,1] = _nodes[x, y];
					if (y > 0) _nodes[x, y-1].neighbours[1,2] = _nodes[x, y];
					if (x < numberOfTiles.x-1) _nodes[x+1, y].neighbours[0,1] = _nodes[x, y];
					if (y < numberOfTiles.y-1) _nodes[x, y+1].neighbours[1,0] = _nodes[x, y];
					if (x > 0 && y > 0) _nodes[x-1, y-1].neighbours[2,2] = _nodes[x, y];
					if (x > 0 && y < numberOfTiles.y-1) _nodes[x-1, y+1].neighbours[2,0] = _nodes[x, y];
					if (x < numberOfTiles.x-1 && y > 0) _nodes[x+1, y-1].neighbours[0,2] = _nodes[x, y];
					if (x < numberOfTiles.x-1 && y < numberOfTiles.y-1) _nodes[x+1, y+1].neighbours[0,0] = _nodes[x, y];
				}
			}
		}

        public IEnumerator AStarAlgorithm()
        {
            int[] startIndex = ConvertIndexAndPosition(startLocation);
            int[] endIndex = ConvertIndexAndPosition(endLocation);
            AStarNode endNode = _nodes[endIndex[0], endIndex[1]];

            currentNode = _nodes[startIndex[0], startIndex[1]];
            currentNode.distanceToEnd = Vector3.Distance(currentNode.worldPosition, endLocation);
            currentNode.CumulativePathDistance = 0;
            openNodes.Add(currentNode);

            CheckNeighbours();
            openNodes.Sort(FCostComparison);
            
            yield return new WaitForEndOfFrame();
            
            currentNode = openNodes[0];
            //move and draw line

            if (currentNode != endNode) StartCoroutine(AStarLoop(endNode));
        }

        private IEnumerator AStarLoop(AStarNode endNode)
        {
            CheckNeighbours(currentNode);
            openNodes.Sort(FCostComparison);
            
            yield return new WaitForEndOfFrame();
            
            currentNode = openNodes[0];
            //move and draw line

            if (currentNode != endNode) StartCoroutine(AStarLoop(endNode));
        }

        //Case: First Node
        private void CheckNeighbours()
        {
            int i = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    AStarNode neighbour = (AStarNode)currentNode.neighbours[x, y];
                    if (neighbour == null) continue;
                    if (neighbour.isBlocked || neighbour.isClosed || openNodes.Contains(neighbour)) continue;
                    i++;
                    openNodes.Add(neighbour);
                    neighbour.distanceToEnd = Vector3.Distance(neighbour.worldPosition, endLocation);
                    neighbour.parent = currentNode;
                    neighbour.CumulativePathDistance =
                            Vector3.Distance(neighbour.worldPosition, currentNode.worldPosition) +
                            currentNode.CumulativePathDistance;
                }
            }
            currentNode.isClosed = true;
            openNodes.Remove(currentNode);
        }
        
        private void CheckNeighbours(AStarNode node)
        {
            int i = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    AStarNode neighbour = (AStarNode)node.neighbours[x, y];
                    if (neighbour == null) continue;
                    if (neighbour.isBlocked || neighbour.isClosed || openNodes.Contains(neighbour)) continue;
                    i++;
                    openNodes.Add(neighbour);
                    neighbour.distanceToEnd = Vector3.Distance(neighbour.worldPosition, endLocation);
                    if (neighbour.CumulativePathDistance < node.parent.CumulativePathDistance)
                    {
                        node.parent = neighbour;
                        node.CumulativePathDistance = Vector3.Distance(neighbour.worldPosition, node.worldPosition) +
                                                      neighbour.CumulativePathDistance;
                    }
                    else
                    {
                        neighbour.parent = node;
                        neighbour.CumulativePathDistance =
                            Vector3.Distance(neighbour.worldPosition, node.worldPosition) +
                            node.CumulativePathDistance;
                    }
                }
            }
            node.isClosed = true;
            openNodes.Remove(node);
        }

        private int FCostComparison(AStarNode x, AStarNode y)
        {
            return Mathf.RoundToInt(y.fCost-x.fCost);
        }


        private Vector3 ConvertIndexAndPosition(int[] index)
        {
            return new Vector3(gridOrigin.x + index[0] * _gridTileSize.x, gridOrigin.y,
                gridOrigin.z + index[1] * _gridTileSize.z);
        }
        
        private int[] ConvertIndexAndPosition(Vector3 position)
        {
            return new [] {Mathf.RoundToInt((position.x-gridOrigin.x)/_gridTileSize.x), 
                Mathf.RoundToInt((position.z-gridOrigin.z)/_gridTileSize.z)};
        }
        
        public Vector3 RandomLocation()
        {
            Vector3 result = new (Random.Range(gridOrigin.x, gridOrigin.x+gridSize.x),gridOrigin.y,
                Random.Range(gridOrigin.z, gridOrigin.z+gridSize.y));
            int[] index = ConvertIndexAndPosition(result);

            if (_nodes[index[0], index[1]].isBlocked)
            {
                result = RandomLocation();
            }
            
            return result;
        }
		
		#endregion

        void Awake()
        {
            FillGrid();
        }
        
        private void OnDrawGizmosSelected()
        {
            for (int x = 0; x < numberOfTiles.x; x++)
            {
                for (int y = 0; y < numberOfTiles.y; y++)
                {
                    if (_nodes == null) return;
                    if (_nodes[x, y] == currentNode)
                    {
                        Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
                        Gizmos.DrawCube(_nodes[x, y].worldPosition, _gridTileSize);
                    }
                    else if (_nodes[x, y].isBlocked)
                    {
                        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                        Gizmos.DrawCube(_nodes[x, y].worldPosition, _gridTileSize);
                    }
                    else if (_nodes[x, y].isClosed)
                    {
                        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                        Gizmos.DrawCube(_nodes[x, y].worldPosition, _gridTileSize);
                    }
                    else if (openNodes.Contains(_nodes[x, y]))
                    {
                        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
                        Gizmos.DrawCube(_nodes[x, y].worldPosition, _gridTileSize);
                    }
                    else
                    {
                        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
                        Gizmos.DrawCube(_nodes[x, y].worldPosition, _gridTileSize);
                    }
                }
            }
        }
    }
}