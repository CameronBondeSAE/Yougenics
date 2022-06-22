using System.Collections;
using System.Collections.Generic;
using Anthill.Effects;
using DG.Tweening;
using UnityEngine;

public class TweenTest : MonoBehaviour
{
    public float zoomLevel;
    public float target;
    public float duration;


    void Start()
    {
        
        DOTween.To(Getter, Setter, target, duration);
        
    }

    private void Setter(float pnenvalue)
    {
        zoomLevel = pnenvalue;

        transform.localPosition = new Vector3(zoomLevel,zoomLevel,1);
        transform.rotation = Quaternion.Euler(zoomLevel, zoomLevel, zoomLevel);
        transform.localScale = new Vector3(zoomLevel, zoomLevel, 1);
        
    }

    private float Getter()
    {
        return zoomLevel;
    }
}
