using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public Food target;
    public Rigidbody rb;
    public float speed = 10;
    // Start is called before the first frame update
    void Update()
    {
        Cometofood();

    }

    // Update is called once per frame
    public void Cometofood()
    {
        target = FindObjectOfType<Food>();
        transform.LookAt(target.transform);
        rb.AddRelativeForce(Vector3.forward * speed);
    }
}
