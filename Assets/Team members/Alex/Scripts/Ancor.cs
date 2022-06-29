using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Ancor : MonoBehaviour
    {
        public Transform camera;
        
        void LateUpdate()
        {
            transform.LookAt(transform.position + camera.forward);
        }
    }
}
