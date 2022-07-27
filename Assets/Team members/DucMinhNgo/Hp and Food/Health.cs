using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace Minh
{
    public class Health : NetworkBehaviour
    {
        public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>();
        public NetworkVariable<bool> IsDead = new NetworkVariable<bool>();

        public int Hp = 100;
        public int curHp;
        public bool dead;
        public bool fullenergy = true;
        public bool noenergy;
        Food food;
        public HpUI hpui;

        public event Action DeathEvent;
        public event Action Collectfood;

        public override void OnNetworkSpawn()
        {
            IsDead.OnValueChanged += UpdateDeadState;
            CurrentHealth.OnValueChanged += UpdateHealth;

            if (IsServer)
            {
                GetComponent<Energy>().NoEnergyEvent += startHealthdepeting;
                GetComponent<Energy>().FullEnergyEvent += startHealthincreasing;
            }
        }

        void Start()
        {
            if (NetworkManager.Singleton == null)
                Debug.Log("No Network Manager Found - ADD ManagerScene For Testing To Your Scene");

            if (hpui != null) hpui.SetMaxHealth(Hp);
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
            }
        }

        public void FullHp()
        {
            //Hp = 100f;
            CurrentHealth.Value = 100f;
        }

        public void Deadtrigger()
        {
            Destroy(gameObject);
        }

        void OnCollisionEnter(Collision collision)
        {
            curHp = Hp -= 80;
            Collectfood?.Invoke();
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

                if (CurrentHealth.Value < 100)
                {
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
            if (hpui != null)
                hpui.SetHealth((int)newValue);

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
