using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float Hp = 100;
    public GameObject coin;
    public bool dead;

    public event Action DeathEvent;
   

    void Start()
    {
        StartCoroutine(addHealth());
        Deathcheck();
        
    }

    IEnumerator addHealth()
    {
        while (true)
        { // loops forever...
            if (Hp < 100)
            { // if health < 100...
                Hp += 1; // increase health and wait the specified time
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
    

}
