using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
    
    [Serializable]
    public struct Node
    {
        public float distance;
    }

    public class ListSort : MonoBehaviour
    {
        public List<Node>    nodes;


        // Start is called before the first frame update
        void Start()
        {
        }

        [Button]
        public void SortTest()
        {
            nodes.Sort(Comparison);
        }

        int Comparison(Node x, Node y)
        {
            if (x.distance > y.distance)
            {
                return -1;
            }
            if (x.distance < y.distance)
            {
                return 1;
            }

            return 0;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}