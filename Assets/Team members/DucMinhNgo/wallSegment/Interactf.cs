using System;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

namespace Minh
{
    public class Interactf : NetworkBehaviour, IItem, IInteractable
    {
        public ItemInfo            itemInfo;
        public Health       health;
        public event Action dealdamage;
        public event Action healing;
        
        // Start is called before the first frame update
        void Awake()
        {
            health.ChangedEvent += Updatewallstatus;
            
            health.DeathEvent += HealthOnDeathEvent;

        }

        void HealthOnDeathEvent()
        {
            GetComponent<NetworkObject>().Despawn(true);
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

        public void     SpawnedAsNormal()
        {
            
        }

        public ItemInfo GetInfo()
        {
            return itemInfo;
        }
    }
}

