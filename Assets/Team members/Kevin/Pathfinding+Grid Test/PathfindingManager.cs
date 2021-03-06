using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

namespace Kevin
{
    public class PathfindingManager : MonoBehaviour
    {
       Node[,] gridNodeReferences;
       public LayerMask unwalkableMask;
       public Vector2 gridWorldSize;
       public float nodeRadius;
       public float nodeDiameter;
       public Transform player; 
       private int gridSizeX, gridSizeY;
       public void Start()
       {
           nodeDiameter = nodeRadius * 2;
           gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
           gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
           CreateGrid();
       }

       void CreateGrid()
       {
           gridNodeReferences = new Node[gridSizeX, gridSizeY];
           Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
                                     Vector3.forward * gridWorldSize.y / 2; 
           for (int x = 0; x < gridSizeX; x++)
           {
               for (int y = 0; y < gridSizeY; y++)
               {
                   Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                        Vector3.forward * (y * nodeDiameter + nodeRadius);
                   bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
                   gridNodeReferences[x, y] = new Node(walkable, worldPoint);
               }
           }
       }

       public Node NodeFromWorldPoint(Vector3 worldPosition)
       {
           float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
           float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
           percentX = Mathf.Clamp01(percentX);
           percentY = Mathf.Clamp01(percentY);

           int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
           int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

           return gridNodeReferences[x,y];
       }
       void OnDrawGizmos()
       {
           Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y));

           if (gridNodeReferences != null)
           {
               Node playerNode = NodeFromWorldPoint(player.position);
               foreach (Node n in gridNodeReferences)
               {
                   Gizmos.color = (n.walkable) ? Color.white : Color.red;
                   if (playerNode == n)
                   {
                       Gizmos.color = Color.cyan; 
                   }
                   Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
               }
           }
       }
    }
}

