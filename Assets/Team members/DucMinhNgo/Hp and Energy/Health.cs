using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartCoroutine(addHealth());
        Deathcheck();
        GetComponent<Energy>().NoEnergyEvent += startHealthdepeting;
        GetComponent<Energy>().FullEnergyEvent += startHealthincreasing;
    }

    //health generate overtime
    IEnumerator addHealth()
    {
        while (noenergy == true)
        { // loops forever...
            if (Hp < 100)
            { // if health < 100...
                Hp += 0.05f; // increase health and wait the specified time
                yield return new WaitForSeconds(1);
            }
            else
            { // if health >= 100, just yield 
                yield return null;
            }
        }
    }
    public void Deathcheck()
    {
        if(Hp <=0)
        {
            dead = true;
            DeathEvent?.Invoke();
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Hp -= 10;
        Collectfood?.Invoke();
    }
    private void Update()
    {
        Deathcheck();
    }
    IEnumerator Healthdepeting()
    {
        while (noenergy == false)
        { // loops forever...
            if (Hp <= 100)
            { // if health < 100...
                Hp -= 0.05f; // increase health and wait the specified time
                yield return new WaitForSeconds(1);
            }
            else
            { // if health >= 100, just yield 
                yield return null;
            }
        }
    }
    void startHealthdepeting()
    {
        
        StartCoroutine(Healthdepeting());
    }
    IEnumerator Healthincreasing()
    {
        while (noenergy == false)
        { // loops forever...
            if (Hp <= 100)
            { // if health < 100...
                Hp -= 0.05f; // increase health and wait the specified time
                yield return new WaitForSeconds(1);
            }
            else
            { // if health >= 100, just yield 
                yield return null;
            }
        }
    }
    void startHealthincreasing()
    {
       
        StartCoroutine(Healthboots());
    }
    IEnumerator Healthboots()
    {
        while (fullenergy == true)
        { // loops forever...
            if (Hp < 100)
            { // if health < 100...
                Hp += 0.05f; // increase health and wait the specified time
                yield return new WaitForSeconds(1);
            }
            else
            { // if health >= 100, just yield 
                yield return null;
            }
        }
    }
}
