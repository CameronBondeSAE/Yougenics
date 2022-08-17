using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    Energy energy;
    
    void Awake()
    {
        energy = GetComponent<Energy>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, Mathf.Sin(Time.time) * .2f, 0)* .2f * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Energy>() != null && !other.GetComponent<EnergyBall>()) // ‘Fire’ the event
        {
            other.GetComponent<Energy>().ChangeEnergy(energy.EnergyAmount.Value);
            Destroy(this.gameObject);
        }
    }
}
