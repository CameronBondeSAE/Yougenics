using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kevin
{
    public class PlannerInterface : MonoBehaviour
    {
        public interface IGlutton
        {
            public void Patrol();
            public void SearchFood();
            public void ChaseFood();
            public void EatFood();
            public void Mitosis();
        }
    }
}

