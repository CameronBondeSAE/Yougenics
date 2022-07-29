using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEditor;

public class Tweeningminimal : MonoBehaviour
{
    public float duration;
    public GameObject minhobject;
    private Vector3 localChange;
    public float newvalue;
    public float adjust1;
        void Start()
        {
            DOTween.To(JustASetter, 0, 1, duration);
        }
        private void JustASetter(float newvalue)
        {
            minhobject.transform.localScale = new Vector3(newvalue, adjust1, 1);
        }
}
