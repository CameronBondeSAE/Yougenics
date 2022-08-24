using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class SteeringBehaviour : MonoBehaviour
    {
        public Vector3 CalculateMove(List<Transform> neighbours)
        {
            if (neighbours.Count == 0) return transform.forward;

            Vector3 alignmentMove = Vector3.zero;

            foreach (Transform n in neighbours)
            {
                alignmentMove += (Vector3) n.transform.transform.forward;
            }

            alignmentMove /= neighbours.Count;

            return alignmentMove;
        }
    }
}

