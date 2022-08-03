using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vectormathallfunction : MonoBehaviour
{
    public Vector3 test;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        
    
        Debug.Log("sqrMagnitude = "+test.sqrMagnitude);
        Debug.Log("normalised = "+test.normalized);
        Debug.Log("magnitude = "+test.magnitude);

        Debug.Log(Vector3.Angle(Vector3.forward, test));
        Debug.Log(Vector3.Cross(Vector3.up, test));
        Debug.Log(Vector3.Dot(Vector3.up, test));
        Debug.Log(Vector3.Reflect(Vector3.up, test));
        Debug.Log(Vector3.ProjectOnPlane(Vector3.up, test));
        Debug.Log(Vector3.Project(Vector3.up, test));
    }


}
