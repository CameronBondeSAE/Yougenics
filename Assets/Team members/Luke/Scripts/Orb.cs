using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public Transform t;
    public float amplitude = 1f;
    public float frequency = 1f;
    public Vector3 baseScale = Vector3.one;
    public float rotationSpeed = 1;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        t = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float scale = amplitude*Mathf.Sin(Time.time*frequency);
        Vector3 scaleVector = Vector3.one*scale+baseScale;
        t.localScale = scaleVector;
        t.Rotate(Vector3.up, rotationSpeed);
    }
}
