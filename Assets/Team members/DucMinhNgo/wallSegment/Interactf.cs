using System;
using System.Collections;
using System.Collections.Generic;
using Kev;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minh
{
    public class Interactf : MonoBehaviour, IInteractable
    {
        public Health health;
        public event Action dealdamage;
        public event Action healing;
        
        // Start is called before the first frame update
        void Awake()
        {
            health.ChangedEvent += Updatewallstatus;
            
        }

        private void Updatewallstatus(float changedamount, GameObject whodidthis)
        {
            throw new NotImplementedException();
        }

        // Update is called once per frame
        public void Updatewallstatus(float health)
        {
            if (health <= 10)
            {
                Healing(); 
            }
            if (health >= 100)
            {
                Interact();    
            }
        }
        public void Healing()
        {
            Debug.Log("repairing");
            health.startHealthincreasing();
            GetComponent<Renderer>().material.color = Color.green;
            healing?.Invoke();
        }
        public void Interact()
        {
            Debug.Log("destroying");
            health.startHealthdepeting();
            GetComponent<Renderer>().material.color = Color.red;
            dealdamage?.Invoke();
        }
    }
}

