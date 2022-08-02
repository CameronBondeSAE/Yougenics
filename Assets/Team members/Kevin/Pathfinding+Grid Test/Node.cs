using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Kevin
{
    public class Node
    {
        public Vector3 worldPosition;
        public bool walkable;

        public Node(bool _walkable, Vector3 _worldPos)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
        }
    }

}
