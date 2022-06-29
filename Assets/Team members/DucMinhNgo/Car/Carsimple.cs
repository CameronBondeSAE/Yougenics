using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Carsimple : MonoBehaviour
{
    public float speed = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {GetComponent<Rigidbody>().AddRelativeForce(0,0, speed );}
        if (Input.GetKeyDown(KeyCode.S))
        {GetComponent<Rigidbody>().AddRelativeForce(0,0, - speed );}
        if (Input.GetKeyDown(KeyCode.A))
        {GetComponent<Rigidbody>().AddRelativeForce(-speed,0, 0  );}

        if (Input.GetKeyDown(KeyCode.D))
        {
            GetComponent<Rigidbody>().AddRelativeForce(speed,0,0 );
            
        }
        


    }
}
