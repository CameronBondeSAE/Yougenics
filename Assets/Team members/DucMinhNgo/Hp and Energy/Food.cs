using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public GameObject food;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().Collectfood += Update;
    }

    // Update is called once per frame
    void Update()
    {      
        
    }
}
