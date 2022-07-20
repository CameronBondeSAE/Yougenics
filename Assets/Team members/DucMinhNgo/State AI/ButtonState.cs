using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace  Minh
{
    [CustomEditor(typeof(Basestate),true)]
    public class ButtonState : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Change state"))
            {
                Minh.Basestate enterstate = target as Minh.Basestate;
                if (enterstate != null)
                {
                    enterstate.stateManager.ChangeState(enterstate);
                }
            }
        }
    }
}

