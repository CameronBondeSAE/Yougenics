using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBodyGen : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject currentGo;
        currentGo = AddLimb();
        
        for (int i = 0; i < 10; i++)
        {
            AddLimb().transform.SetParent(currentGo.transform);
        }
    }

    public GameObject AddLimb()
    {
        GameObject go           = new GameObject();
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        go.AddComponent<MeshRenderer>();
        go.AddComponent<Rigidbody>();
        go.AddComponent<Wibbler>();
        return go;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
