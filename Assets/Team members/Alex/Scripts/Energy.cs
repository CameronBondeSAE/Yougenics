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

    public float energyAmount = 50;
    public float energyMax = 100;
    public float energyMin = 0f;
    public float drainAmount = 1;
    public event Action NoEnergyEvent; 
    public event Action FullEnergyEvent;
    public float drainSpeed = 1;
    public bool energyUserMoving = false;

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
        StartCoroutine(EnergyDrainer());
        
    }

    private void FixedUpdate()
    {
        Debug.Log(GetComponent<Rigidbody>().velocity.magnitude);
        if (GetComponent<Rigidbody>().velocity.magnitude > 5f)
        {
            energyUserMoving = true;
            StartCoroutine(MovementEnergyDrain());
            if (GetComponent<Rigidbody>().velocity.magnitude < .001f)
            {
                energyUserMoving = false; 
                StopCoroutine(MovementEnergyDrain());
            }
        }
    }

    public void ChangeEnergy(float f)
    {
        if (IsServer)
            EnergyAmount.Value += f;

        //energyAmount = energyAmount + f;
        //CheckEnergyMax();
        //CheckEnergyMin();
    }

    public void CheckEnergyMax()
    {
        if (EnergyAmount.Value >= energyMax)
        {

            //EnergyAmount.Value = energyMax;
            FullEnergyEvent?.Invoke();
            Debug.Log("Full Energy");
            //FindObjectOfType<AudioManager>().Play("Energy Full");
        }
    }

    public void CheckEnergyMin()
    {
        if (EnergyAmount.Value <= energyMin)
        {
            //EnergyAmount.Value = energyMin;

            NoEnergyEvent?.Invoke();
            Debug.Log("No Energy");

        }
        //Debug.Log("Editor test");
    }
    
    public IEnumerator EnergyDrainer()
    {
        while (EnergyAmount.Value > energyMin)
        {
            yield return new WaitForSeconds(drainSpeed);
            {
                if(IsServer)
                    EnergyAmount.Value -= drainAmount;
                //else
                    //RequestEnergyDrainServerRpc()
            }
        }
    }

    public IEnumerator MovementEnergyDrain()
    {
        if(IsServer)
            yield return new WaitForSeconds(Time.deltaTime);
            EnergyAmount.Value -= drainAmount * Time.deltaTime;
        
    }
        

    #region Networking Implementation

    private void CheckEnergy(float previousValue, float newValue)
    {
        if (previousValue < newValue)
            CheckEnergyMax();
        else
            CheckEnergyMin();
    }

    [ServerRpc]
    private void RequestEnergyDrainServerRpc()
    {
        EnergyAmount.Value -= drainAmount;
    }

    #endregion
}
