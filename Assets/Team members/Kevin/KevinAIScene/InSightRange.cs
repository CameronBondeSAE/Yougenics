using System.Collections;
using System.Collections.Generic;
using Kevin;
using UnityEngine;

public class InSightRange : MonoBehaviour
{
    public GameObject foxObject;
    public FoxModel _foxModel;
    
    void Start()
    {
        _foxModel = foxObject.GetComponent<FoxModel>();
        //insightRange = _foxModel.insightRange;
        this.GetComponent<SphereCollider>().radius = _foxModel.insightRange; 
    }
    
    public void OnTriggerEnter(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();
        if (entity != null)
        {
            Debug.Log("Collide");
            _foxModel.entityInSightRange = true;
            _foxModel.surroundingEntities.Add(other.transform); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();
        if (entity != null)
        {
            Debug.Log("!Collide");
            _foxModel.entityInSightRange = false;
            _foxModel.surroundingEntities.Remove(other.transform); 
        }
    }
}
