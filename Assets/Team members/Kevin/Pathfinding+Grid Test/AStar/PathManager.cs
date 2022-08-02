using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class PathManager : MonoBehaviour
    {
        Node[,] gridNodeReferences;
        
        public LayerMask unwalkable;
        
        public Vector2 gridWorldSize;
        
        public float nodeRadius;
        public float nodeDiameter;

        private int gridSizeX, gridSizeY;

        public Vector3 gridTileSize;
        public void Start()
        {
            nodeDiameter = nodeRadius * 2; 
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        public void CreateGrid()
        {
            gridNodeReferences = new Node[gridSizeX, gridSizeY];
            gridTileSize = new Vector3(gridWorldSize.x/gridSizeX, gridWorldSize.y, gridWorldSize.y/gridSizeY);
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if (Physics.OverlapBox(gridNodeReferences[x, y].worldPosition, gridTileSize * 0.5f,
                            Quaternion.identity, unwalkable).Length != 0)
                    {
                        gridNodeReferences[x, y].walkable = false; 
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (gridNodeReferences != null)
            {
                foreach (Node n in gridNodeReferences)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                }
            }
        }
    }
}

