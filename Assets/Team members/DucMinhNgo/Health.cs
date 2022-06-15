using System;
using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;

namespace Minh
{
    public class Health : MonoBehaviour
    {
        public float Hp = 100f;
        public bool dead;
        public bool fullenergy = true;

        public event Action DeathEvent;
        Food food;
        public event Action Collectfood;
        public bool noenergy;

        void Start()
        {
            noenergy = false;
            fullenergy = true;
            
            Deathcheck();
            GetComponent<Energy>().NoEnergyEvent += startHealthdepeting;
            GetComponent<Energy>().FullEnergyEvent += startHealthincreasing;
        }

        //health generate overtime
        

        public void Deathcheck()
        {
            if (Hp <= 0)
            {
                dead = true;
                GetComponent<Renderer>().material.color = Color.yellow;
                DeathEvent?.Invoke();
                
            }
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
