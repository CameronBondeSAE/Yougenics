using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class Pathfinding : MonoBehaviour
    {
        private PathfindingManager _pathfindingManager;

        public void Awake()
        {
            _pathfindingManager = GetComponent<PathfindingManager>();
        }

        void FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            Node startNode = _pathfindingManager.NodeFromWorldPoint(startPosition);
            Node targetNode = _pathfindingManager.NodeFromWorldPoint(targetPosition);
        }
    }
}

