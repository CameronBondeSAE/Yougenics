using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This only exists because GameObjects CAN'T SPAWN instances of THEIR ORIGINAL prefab, it remaps to the spawned instance
/// </summary>
[CreateAssetMenu]
public class PrefabReferenceHack : ScriptableObject
{
    public GameObject prefab;
}
