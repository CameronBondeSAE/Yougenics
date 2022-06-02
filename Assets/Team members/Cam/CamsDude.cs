using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamsDude : MonoBehaviour
{
    public bool doingThing;
    public float amount;


    public GameObject otherThing;


    void Start()
    {
        // I am interested in this event, so I 'subscribe/listen/observe' to the event
        // GetComponent<Health>().DeathEvent += CamSuperDeath;
        GetComponent<Energy>().FullEnergyEvent += Hyper;
        GetComponent<Energy>().NoEnergyEvent += FindFood;
    }

    void FindFood()
    {
        Debug.Log("Find food");
    }

    void Hyper()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }


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

    public void CamSuperDeath()
    {
        // Play crazy sound
        // Animate
        // Wait for 5 seconds
        Destroy(gameObject);
    }
}
