using System.Collections;
using System.Collections.Generic;
using Cam;
using UnityEngine;


    public class FoodTest : MonoBehaviour, IEdible
    {
        [SerializeField] private CommonAttributes commonAttributes;

        public void Awake()
        {
            commonAttributes = GetComponent<CommonAttributes>(); 
        }
        public void OnEnable()
        {
            commonAttributes.dangerLevel = 0f; 
        }

        #region IEdible

        public float GetEnergyAmount()
        {
            throw new System.NotImplementedException();
        }

        public float EatMe(float energyRemoved)
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }


