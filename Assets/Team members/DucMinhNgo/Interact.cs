using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minh
{
    public class Interact : MonoBehaviour
    {
        public float hp = 100;
        public float curhp;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Dealdamage();
        }

        void Dealdamage()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                curhp = hp - 10;
                GetComponent(Renderer)
            }
        }
        
    }
}

