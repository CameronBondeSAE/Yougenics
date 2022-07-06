using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class NodeBase
	{
		public NodeBase[,] neighbours = new NodeBase[3,3];
        public Vector3 worldPosition;
	}
}