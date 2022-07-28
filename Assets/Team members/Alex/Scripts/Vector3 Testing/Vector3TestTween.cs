using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Vector3TestTween : MonoBehaviour
{
    public float speed = 10f;
    

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
    }
}
