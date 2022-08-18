using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawline : MonoBehaviour
{
    public Vector3 test;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        Vector3 reflect       = Vector3.Reflect(Vector3.up, test);
        Vector3 startPosition = new Vector3(3, 5, 0);

        Debug.DrawLine(startPosition, reflect, Color.green, 5f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
