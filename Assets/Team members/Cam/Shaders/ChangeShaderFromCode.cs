using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShaderFromCode : MonoBehaviour
{
    public Gradient gradient;
    
    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Renderer>().material.set("_Gradient");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.SetFloat("_FlamePhase", Mathf.PerlinNoise(Time.time+transform.position.x,0)*30f);
    }
}
