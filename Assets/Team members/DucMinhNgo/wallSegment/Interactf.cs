using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
            if (changedamount > 0f)
            {
                Healing(); 
            }
            if (changedamount < 0f)
            {
                Interact();    
            }
        }
        public void Healing()
        {
            Debug.Log("repairing");
            if (DOTween.IsTweening(GetComponent<Renderer>().material))
            {
                DOTween.Kill(GetComponent<Renderer>().material, true);
            }

            GetComponent<Renderer>().material.color = Color.green;
            GetComponent<Renderer>().material.DOColor(Color.gray, 3f);
            healing?.Invoke();
        }
        public void Interact()
        {
            Debug.Log("destroying");
            if (DOTween.IsTweening(GetComponent<Renderer>().material))
            {
                DOTween.Kill(GetComponent<Renderer>().material, true);
            }

            GetComponent<Renderer>().material.color = Color.red;
            GetComponent<Renderer>().material.DOColor(Color.gray, 3f);


        }
    }
}

