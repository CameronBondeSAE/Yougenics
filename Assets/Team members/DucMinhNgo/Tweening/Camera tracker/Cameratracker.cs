using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace  Minh
{
    public class Cameratracker : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;

        public float speed = 10f;
    
        void LateUpdate()
        {
            transform.position = Vector3.Slerp(transform.position, target.position + offset, speed * Time.deltaTime);
            transform.LookAt(target.position);
        }
    }
}

