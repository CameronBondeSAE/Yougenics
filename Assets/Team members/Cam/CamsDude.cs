using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamsDude : MonoBehaviour
{
    public bool doingThing;
    public float amount;

    public bool CheckThing()
    {
        return true;
    }

    public void DoThing()
    {
        Debug.Log("I did a thing");
    }

    public void DoMoreThing(bool what)
    {
        Debug.Log("What = "+what);
    }
}
