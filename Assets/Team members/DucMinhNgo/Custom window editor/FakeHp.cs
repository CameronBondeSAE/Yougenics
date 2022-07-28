using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;

namespace Minh
{
    public class FakeHp : MonoBehaviour
    {
        public int HP;
        // Start is called before the first frame update
        void Start()
        {
           
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            dealdmg();
        }

        void dealdmg()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                HP -= 92;
            }
        }
    }
}
