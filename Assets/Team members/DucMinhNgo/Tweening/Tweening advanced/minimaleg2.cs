using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class minimaleg2 : MonoBehaviour
{
    public float zoomLevel;
    public float animationspeed;
    public float duration;
    public GameObject tween;
    public float width;

    void Start()
    {
        DOTween.To(Getter, Setter, animationspeed, duration).SetEase(Ease.OutBounce).OnComplete(Finish);
    }

    private void Setter(float pnewvalue)
    {
        zoomLevel = pnewvalue;
        tween.transform.localScale = new Vector3(zoomLevel, width , 1);
        

    }
    private float Getter()
    {
        return zoomLevel;
    }

    public void Finish()
    {
        Debug.Log("Animation finish");
    }
}
