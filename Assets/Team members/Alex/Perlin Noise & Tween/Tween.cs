using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TreeEditor;
using UnityEngine;

public class Tween : MonoBehaviour
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
    }

    private float Getter()
    {
        return zoomLevel;
    }

    private void MoveTest()
    {
        
    }
    
}