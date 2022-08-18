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
    public float length;
    public float width;
        public void Start()
        {
            DOTween.To(Setter,width, length, duration);
        }
        private void Setter(float newvalue)
        {
            minhobject.transform.localScale = new Vector3(length, width, 1);
        }
}
