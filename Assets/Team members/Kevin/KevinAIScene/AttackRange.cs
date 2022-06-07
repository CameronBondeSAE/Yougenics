using System.Collections;
using System.Collections.Generic;
using Kevin;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    
    public GameObject foxObject;
    public FoxModel _foxModel;
    
    void Start()
    {
        _foxModel = foxObject.GetComponent<FoxModel>();
        this.GetComponent<SphereCollider>().radius = _foxModel.attackRange; 
    }

    public void OnTriggerEnter(Collider other)
    {
        IEdible edible = other.GetComponent<IEdible>();
        if (edible != null)
        {
            _foxModel.entityInSightRange = true;
            _foxModel.entityInAttackRange = true; 
        }
    }

    void OnTriggerExit(Collider other)
    {
        IEdible edible = other.GetComponent<IEdible>();
        if (edible != null)
        {
            _foxModel.entityInSightRange = true;
            _foxModel.entityInAttackRange = false; 
        }
    }
}
