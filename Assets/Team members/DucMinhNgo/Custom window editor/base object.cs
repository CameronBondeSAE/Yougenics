using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseobject : MonoBehaviour
{
    public GameObject Car;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Car);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
