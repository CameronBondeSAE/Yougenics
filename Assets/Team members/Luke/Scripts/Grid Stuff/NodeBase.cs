using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	[Serializable]
	public class NodeBase
	{
        public Vector3 worldPosition;
        public int[] indices = new int[2];
	}
}