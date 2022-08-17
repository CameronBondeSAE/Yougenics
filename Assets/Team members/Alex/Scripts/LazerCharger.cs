using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;
using UnityEngine.InputSystem.HID;


namespace Alex
{
    public class LazerCharger : MonoBehaviour, IInteractable
    {
        Gun lazer;
        public Button button;

        // Start is called before the first frame update
        void Start()
        {
            lazer = GetComponent<Gun>();
        }
        

        void Charge()
        {
            lazer.GetComponent<Energy>().ChangeEnergy(lazer.GetComponent<Energy>().energyMax);
        }

        public void Interact()
        {
            button.buttonPressedEvent += Charge;
        }
    }
}