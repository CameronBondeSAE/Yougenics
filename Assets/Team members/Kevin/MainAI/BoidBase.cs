using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBase : MonoBehaviour
{
    public List<Transform> neighbours;

    public void AddNeighbour(Collider other)
    {
        neighbours.Add(other.transform);
    }

    public void RemoveNeighbour(Collider other)
    {
        neighbours.Remove(other.transform);
    }
}
