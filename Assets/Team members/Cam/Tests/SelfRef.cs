using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Tanks;
using UnityEngine;

public class SelfRef : MonoBehaviour
{
    public PrefabReferenceHack camsPrefabReferenceHack;
    
    [Button]
    void Spawn()
    {
        GameObject instantiate = Instantiate(camsPrefabReferenceHack.prefab);
        instantiate.transform.position = Vector3.one;
    }
}
