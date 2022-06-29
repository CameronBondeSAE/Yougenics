using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{ 
    public class mayAstar : MonoBehaviour
    {
        public Vector3 currentNode;
        public List<Node> openNodes;
        public List<Node> closedNodes;
        public Node[,] neighbours;
    }
}
