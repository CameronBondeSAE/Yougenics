using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class minimaleg2 : MonoBehaviour
{
    public float zoomLevel;
    public float target;
    public float duration;
    public Vector3 localScale;
    public GameObject tween;
    public float adjust1;

    void Start()
    {
        DOTween.To(Getter, Setter, target, duration);
    }

    private void Setter(float pnewvalue)
    {
        zoomLevel = pnewvalue;
        tween.transform.localScale = new Vector3(zoomLevel, adjust1 , 1);
    }
    private float Getter()
    {
        return zoomLevel;
    }
}
