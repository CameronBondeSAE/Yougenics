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
        public NetworkVariable<bool> FullEnergy = new NetworkVariable<bool>();
        public NetworkVariable<bool> NoEnergy = new NetworkVariable<bool>();

        public int Hp = 100;
        public int curHp;
        public bool dead;
        public bool fullenergy = true;
        public bool noenergy;
        Food food;
        public HpUI hpui;

        public event Action DeathEvent;
        public event Action Collectfood;

        void Start()
        {
            
            hpui.SetMaxHealth(Hp);
            //noenergy = false;
            //fullenergy = true;
            NoEnergy.Value = false;
            FullEnergy.Value = true;
            
            Deathcheck();
            GetComponent<Energy>().NoEnergyEvent += startHealthdepeting;
            GetComponent<Energy>().FullEnergyEvent += startHealthincreasing;
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
            curHp = Hp -= 80;
            Collectfood?.Invoke();
        }

        private void FixedUpdate()
        {
            Deathcheck();
            curHp = Hp;
        }
        
        // health increase and depleting overtime code
        IEnumerator Healthdepleting()
        {
            while (noenergy)
            {
                // loops forever...
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
                hpui.SetHealth(curHp);
            }
        }

       public void startHealthdepeting()
        {

            StartCoroutine(Healthdepleting());
        }
        public void startHealthincreasing()
        {

            StartCoroutine(Healthincreasing());
        }
        IEnumerator Healthincreasing()
        {
            while (fullenergy)
            {
                // loops forever...
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
                hpui.SetHealth(curHp);
            }
        }
    }

}
