using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CamsGuy
{
    public bool   isCool;
    public float  coolness;
    public string coolName;
    public bool   isAmazing;
}

public class FileSaving : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Pretend this exists (just use a real GameObject)
        CamsGuy camsGuy = new CamsGuy();

        string json = JsonUtility.ToJson(camsGuy, true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
