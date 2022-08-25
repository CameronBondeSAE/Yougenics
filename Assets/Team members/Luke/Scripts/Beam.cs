using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public Transform t;
    public Transform target;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 position = t.position;
        Vector3 targetPosition = target.position;
        t.localScale = new Vector3(1, 1, Vector3.Distance(position, targetPosition));
        t.rotation = Quaternion.LookRotation(targetPosition-position, Vector3.up);
    }
	private void OnEnable()
    {
        t = GetComponent<Transform>();
		Destroy(gameObject, 2f);
	}
}
