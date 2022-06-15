using System.Collections;
using System.Collections.Generic;
using Kev;
using UnityEngine;

public class ActionSphere : MonoBehaviour
{
    public GameObject critterPrefab;
    public CritterBase critterBase;
    public void Awake()
    {
        //Get the component of the specific critter script from the prefab assigned.
        //Figure out how to not do it this way later
        critterBase = critterPrefab.GetComponent<CritterBase>();
        GetComponent<SphereCollider>().radius = critterBase.actionRadius;
        GetComponent<SphereCollider>().center = new Vector3(0f, 0f, 0.8f);
        Physics.IgnoreLayerCollision(6,6);

        
    }
    
    public void OnTriggerEnter(Collider other)
    {
        CritterBase otherCritter = other.GetComponent<CritterBase>();
        Interface.Foodchain foodChain = critterPrefab.GetComponent<CritterBase>().foodChain;
        Interface.Gender gender = critterPrefab.GetComponent<CritterBase>().gender;
        Interface.IEdible edible = GetComponent<Interface.IEdible>();
        
        critterBase.entityInAttackRange = true;
        
        //prey and neutral dead
        if(otherCritter.foodChain == Interface.Foodchain.Predator && foodChain is Interface.Foodchain.Neutral or Interface.Foodchain.Prey)
        {
            critterBase.Death();
            critterBase.ListRemover(other.transform);
            //Debug.Log("running!");
        }
        
        //neutral kill prey
        if (otherCritter.foodChain == Interface.Foodchain.Neutral && foodChain == Interface.Foodchain.Prey)
        {
            critterBase.Death();
            critterBase.ListRemover(other.transform);
        }
        
        //predators attack neutrals
        if (otherCritter.foodChain == Interface.Foodchain.Neutral && foodChain == Interface.Foodchain.Predator)
        {
            //attack neutral
            otherCritter.Death();
            critterBase.ListRemover(other.transform);
            //Debug.Log("attacking a neutral");
        }
        //predators and neutrals attack any preys
        if (otherCritter.foodChain == Interface.Foodchain.Prey && foodChain != Interface.Foodchain.Prey)
        {
             //attack 
             otherCritter.Death();
             critterBase.ListRemover(other.transform);
             //Debug.Log("attacking all preys");
        }
        
        //Mate if the same critter type
        if (otherCritter.foodChain == foodChain && otherCritter.gender != gender)
        {
            critterBase.isMating = true;
            otherCritter.isMating = true;
            //critterBase.horny = true;
            //otherCritter.BeginMate();
        }

        if (edible != null)
        {
            critterBase.BeginEat(other);
        }
        //run functions depending on what critter type
    }
        
    public void OnTriggerExit(Collider other)
    {
        critterBase.entityInAttackRange = false;
    }
}
