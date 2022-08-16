using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using Tanks;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace Minh
{
    public class Health : NetworkBehaviour
    {
        public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>();
        public NetworkVariable<bool> IsDead = new NetworkVariable<bool>();

        public float maxHealth = 100f;
        
        //public int Hp = 100;
        //public int curHp;
        //public bool dead;
        public bool fullenergy = true;
        public bool noenergy = false;
        
        

        public event Action          DeathEvent;
        public delegate void         ChangedDelegate(float changedAmount, GameObject whoDidThis);
        public event ChangedDelegate ChangedEvent;

        public override void OnNetworkSpawn()
        {
            IsDead.OnValueChanged += UpdateDeadState;
            CurrentHealth.OnValueChanged += UpdateHealth;

            if (IsServer)
            {
                // GetComponent<Interactf>().healing += startHealthincreasing;
                // GetComponent<Interactf>().dealdamage += startHealthdepeting;
                GetComponent<Energy>().NoEnergyEvent += startHealthdepeting;
                GetComponent<Energy>().FullEnergyEvent += startHealthincreasing;
            }
        }
        public void Awake()
        {
            startHealthincreasing();
            
            
        }
        
        void Start()
        {
            if (NetworkManager.Singleton == null)
                Debug.Log("No Network Manager Found - ADD ManagerScene For Testing To Your Scene");
            
        }

        public void ChangeHealth(float amount, GameObject whoDidThis)
        {
            if (IsServer)
            {
                CurrentHealth.Value += amount;
                if (CurrentHealth.Value > maxHealth)
                {
                    CurrentHealth.Value = maxHealth;
                }
                ChangedEvent?.Invoke(amount, whoDidThis);
            }
        }
        
        public void ChangeHealth(float amount)
        {
            ChangeHealth(amount, null);
        }

        public void Deathcheck()
        {
            /*if (Hp <= 0)
            {

                dead = true;
                GetComponent<Renderer>().material.color = Color.yellow;
                Destroy(gameObject);
                DeathEvent?.Invoke();

            }*/

            if (CurrentHealth.Value <= 0)
            {
                IsDead.Value = true;
                DeathEvent?.Invoke();
            }
        }

        public void FullHp()
        {
            //Hp = 100f;
            ChangeHealth(100000);
        }

        public void Deadtrigger()
        {
            // CAM NOTE: Nope, we can't assume how a Critter will die, so we can't do things like Destroy the gameObject. What if they want to play an animation with a cool sound? What if they split in two or something weird? We'll leave that up to the critter AI programmer, so all we can really do is invoke an event and let them react to it.
            // ChangeHealth();
            //Destroy(gameObject);
        }

        // health increase and depleting overtime code
        IEnumerator Healthdepleting()
        {
            while (noenergy)
            {
                /*// loops forever...
                if (Hp <= 100)
                {
                    // if health < 100...
                    Hp -= 1; // increase health and wait the specified time
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    // if health >= 100, just yield 
                    yield return null;
                }
                hpui.SetHealth(curHp);*/


                //if we have health
                if (CurrentHealth.Value > 0)
                {
                    if(IsServer)
                        CurrentHealth.Value -= 1;
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    // if health >= 100, just yield 
                    yield return null;
                }
            }
        }

        public void startHealthdepeting()
        {
            noenergy = true;
            StartCoroutine(Healthdepleting());
        }
        public void startHealthincreasing()
        {
            fullenergy = true;
            StartCoroutine(Healthincreasing());
        }
        IEnumerator Healthincreasing()
        {
            while (fullenergy)
            {
                noenergy = false;
                /*// loops forever...
                if (Hp <= 100)
                {
                    // if health < 100...
                    Hp += 1; // increase health and wait the specified time
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    // if health >= 100, just yield 
                    yield return null;
                }
                hpui.SetHealth(curHp);*/

                if (CurrentHealth.Value < maxHealth)
                {
                    if(IsServer)
                        CurrentHealth.Value += 1;
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    // if health >= 100, just yield 
                    yield return null;
                }
            }
        }

        #region Networking Implementation

        private void UpdateDeadState(bool previousValue, bool newValue)
        {
            Debug.Log(gameObject.name + " has died");
            DeathEvent?.Invoke();
        }

        private void UpdateHealth(float previousValue, float newValue)
        {

            if (IsServer)
                Deathcheck();
            else
                RequestDeathCheckServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestDeathCheckServerRpc()
        {
            Deathcheck();
        }

        #endregion
    }

}
