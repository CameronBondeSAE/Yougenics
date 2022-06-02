using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int curHealth = 10;

    void Start()
    {
        StartCoroutine(addHealth());
    }

    IEnumerator addHealth()
    {
        while (true)
        { // loops forever...
            if (curHealth < 100)
            { // if health < 100...
                curHealth += 1; // increase health and wait the specified time
                yield return new WaitForSeconds(1);
            }
            else
            { // if health >= 100, just yield 
                yield return null;
            }
        }
    }



}
