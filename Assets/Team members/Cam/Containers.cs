using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Containers : MonoBehaviour
{
    public GameObject camDude;
    
    public GameObject[] intArray;
    public List<GameObject> intList;
    public Dictionary<int, GameObject> intDictionary;

    // Start is called before the first frame update
    void Start()
    {
        intDictionary.Add(42341416, camDude);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
