using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Alex;
using Unity.Netcode;


public class Energy : NetworkBehaviour
{
    //[SerializeField]
    public NetworkVariable<float> EnergyAmount = new NetworkVariable<float>();
    public float        energyMax       = 100;
    public float        energyMin       = 0f;
    public float        drainAmount     = 1;
    public float        moveDrainAmount = 1f;
    public event Action NoEnergyEvent; 
    public event Action FullEnergyEvent;
    public event Action<float> EnergyChangedEvent;
    
    
    public float drainSpeed = 1;
    public bool energyUserMoving = false;
    private Rigidbody rb;
    public float myMagnitude;

    public override void OnNetworkSpawn()
    {
        EnergyAmount.OnValueChanged += CheckEnergy;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (NetworkManager.Singleton == null)
            Debug.Log("No Network Manager Found - ADD ManagerScene For Testing To Your Scene");

        energyUserMoving = false;
        GetComponent<Minh.Health>();
        CheckEnergyMax();
        CheckEnergyMin();
        StartCoroutine(EnergyDrainer());
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        myMagnitude = rb.velocity.magnitude;
        if (myMagnitude > 1f)
        {
            MovementEnergyDrain();
        }
    }

    public float ChangeEnergy(float f)
    {
        if (IsServer)
        {
            if (f < 0)
            {
                if (EnergyAmount.Value < f)
                {
                    return EnergyAmount.Value;
                }
                else
                {
                    return EnergyAmount.Value += f;
                }
            }

            if (f >= 0)
            {
                if (f > energyMax)
                {
                    return EnergyAmount.Value = energyMax;
                }
                else
                {
                    return EnergyAmount.Value += f;
                }
            }
        }

        return 0;

        //energyAmount = energyAmount + f;
        //CheckEnergyMax();
        //CheckEnergyMin();
    }

    public void CheckEnergyMax()
    {
        if (EnergyAmount.Value >= energyMax)
        {

            EnergyAmount.Value = energyMax;
            FullEnergyEvent?.Invoke();
        }
    }

    public void CheckEnergyMin()
    {
        if (EnergyAmount.Value <= 0)
        {
            EnergyAmount.Value = 0;

            NoEnergyEvent?.Invoke();
        }
    }
    
    public IEnumerator EnergyDrainer()
    {
        while (EnergyAmount.Value > energyMin)
        {
            yield return new WaitForSeconds(drainSpeed);
            {
                //if(IsServer)
                    EnergyAmount.Value -= drainAmount;
                //else
                    //RequestEnergyDrainServerRpc()
            }
        }
    }

    public void MovementEnergyDrain()
    {
        //if(IsServer)
        
        EnergyAmount.Value -= moveDrainAmount;
        
    }
        

    #region Networking Implementation

    private void CheckEnergy(float previousValue, float newValue)
    {
        if (previousValue < newValue)
            CheckEnergyMax();
        else
            CheckEnergyMin();
        
        EnergyChangedEvent?.Invoke(EnergyAmount.Value);
    }

    [ServerRpc]
    private void RequestEnergyDrainServerRpc()
    {
        EnergyAmount.Value -= drainAmount;
    }

    #endregion
}
