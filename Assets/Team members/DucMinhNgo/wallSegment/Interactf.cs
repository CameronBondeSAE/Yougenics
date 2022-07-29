using System;
using System.Collections;
using System.Collections.Generic;
using Kev;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minh
{
    public class Interactf : MonoBehaviour, IInteractable
    {
        public HpUI hpui;
        public int hp = 90;
        public int curhp = 90;
        public event Action dealdamage;
        public event Action healing; 
        
        // Start is called before the first frame update
        void FixedUpdate()
        {
            Dealdamage();
            Interact();
            if (hpui != null) hpui.SetMaxHealth(hp);
        }

        // Update is called once per frame


        public void Dealdamage()
        {
           
                hp = curhp = hp - 10;
                GetComponent<Renderer>().material.color = Color.red;
                if (hp == 0)
                {
                    Destroy(gameObject);
                }
                hpui.SetMaxHealth(hp);
                dealdamage?.Invoke();
        }

        public void Interact()
        {
            hp = curhp = hp + 10;
            if (hp >= 100)
            {
                GetComponent<Renderer>().material.color = Color.green;
                curhp = 100;
                Debug.Log("Full Hp");
            }
            hpui.SetMaxHealth(hp);
            healing?.Invoke();
        }
    }
}

