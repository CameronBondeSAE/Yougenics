using System.Collections;
using System.Collections.Generic;
using Alex;
using Kevin;
using UnityEngine;
using UnityEngine.InputSystem.HID;


namespace Alex
{
    public class LazerCharger : MonoBehaviour, IInteractable
    {
        LazerGun lazer;
        public Button button;

        // Start is called before the first frame update
        void Start()
        {
            lazer = GetComponent<LazerGun>();
        }
        

        public void Charge()
        {
            lazer.energy.ChangeEnergy(lazer.energy.energyMax);
            GameManager.instance.energy.EnergyAmount.Value -=
                (lazer.energy.energyMax - lazer.energy.EnergyAmount.Value);
        }

        public void Interact()
        {
            button.buttonPressedEvent += Charge;
        }
    }
}