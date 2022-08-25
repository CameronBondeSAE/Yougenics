using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Ollie
{
    public class StateViewInfo
    {
        public string text;
        public Material particleMaterial;
        public AudioClip audioClip;
    }
    public class StateDictionary : SerializedMonoBehaviour
    {
        public Dictionary<Type, StateViewInfo> aiStateInfo;
    }
}
