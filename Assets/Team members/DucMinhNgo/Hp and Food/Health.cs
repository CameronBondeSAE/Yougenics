using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using Unity.Netcode;

namespace Minh
{
    public class Health : NetworkBehaviour
    {
        public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>();
        public NetworkVariable<bool> IsDead = new NetworkVariable<bool>();
        public NetworkVariable<bool> FullEnergy = new NetworkVariable<bool>();
        public NetworkVariable<bool> NoEnergy = new NetworkVariable<bool>();

        public float Hp = 100f;
        public bool dead;
        public bool fullenergy = true;
        public bool noenergy;
        Food food;

        public event Action DeathEvent;
        public event Action Collectfood;

        void Start()
        {
            //noenergy = false;
            //fullenergy = true;
            NoEnergy.Value = false;
            FullEnergy.Value = true;
            
            Deathcheck();
            GetComponent<Energy>().NoEnergyEvent += startHealthdepeting;
            GetComponent<Energy>().FullEnergyEvent += startHealthincreasing;
        }

        //health generate overtime
        

        public void Deathcheck()
        {
            /*if (Hp <= 0)
            {

                dead = true;
                GetComponent<Renderer>().material.color = Color.yellow;
                Destroy(gameObject);
                DeathEvent?.Invoke();

            }*/

            if (!IsOwner)
                return;

            if (CurrentHealth.Value <= 0)
            {

                IsDead.Value = true;
                //GetComponent<Renderer>().material.color = Color.yellow;
                Destroy(gameObject);
                DeathEvent?.Invoke();

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
            Hp -= 0;
            Collectfood?.Invoke();
        }

        private void Update()
        {
            Deathcheck();
        }
        
        // health increase and depleting overtime code
        IEnumerator Healthdepleting()
        {
            while (noenergy == true)
            {
                // loops forever...
                if (Hp <= 100)
                {
                    // if health < 100...
                    Hp -= 1f; // increase health and wait the specified time
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    // if health >= 100, just yield 
                    yield return null;
                }
            }
        }

        void startHealthdepeting()
        {

            StartCoroutine(Healthdepleting());
        }
        void startHealthincreasing()
        {

            StartCoroutine(Healthincreasing());
        }
        IEnumerator Healthincreasing()
        {
            while (fullenergy == true)
            {
                // loops forever...
                if (Hp <= 100)
                {
                    // if health < 100...
                    Hp += 1f; // increase health and wait the specified time
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    // if health >= 100, just yield 
                    yield return null;
                }
            }
        }
    }

}
