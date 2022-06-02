using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int Hp = 10;
    public GameObject coin;
   

    void Start()
    {
        StartCoroutine(addHealth());
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
    private void OnTriggerEnter(Collider other)
    {
        Hp -= 10;
        if(Hp <= 0)
        {
            DestroyObject(gameObject);
        }
        
    }
}
