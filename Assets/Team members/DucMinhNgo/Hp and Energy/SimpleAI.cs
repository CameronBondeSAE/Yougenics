using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public Food target;
    public Rigidbody rb;
    public float speed = 1000;
    // Start is called before the first frame update
    void Update()
    {
        Findfood();
    }
    void Findfood()
    {
        target = FindObjectOfType<Food>();
        transform.LookAt(target.transform);
        rb.AddRelativeForce(0, 0, speed);
    }

    // Update is called once per frame

}
