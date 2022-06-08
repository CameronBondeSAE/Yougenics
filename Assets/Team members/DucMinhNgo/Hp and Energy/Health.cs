using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float Hp = 100f;
    public bool dead;

    public event Action DeathEvent;
    Food food;
    public event Action Collectfood;
   

    void Start()
    {
        StartCoroutine(addHealth());
        Deathcheck();
        Foodcollect();
        
    }

    //health generate overtime
    IEnumerator addHealth()
    {
        while (true)
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
    public void Foodcollect()
    {
        void OnCollisionEnter(Collision collision)
        {
            Hp += 5;
            Collectfood?.Invoke();
        }
            
    }
    private void Update()
    {
        Deathcheck();
    }

}
