using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Alex
{
    public class ItemInfoUI : MonoBehaviour
    {
        public ShopSingleItem shopSingleItem;
        string name;
        string description;
        float energyRequired;
        float buildTime;
        float timeRemaning;
        float startTime;
        TMP_Text textMeshPro;

        void Start()
        {
            ResetTimer();
            textMeshPro = GetComponent<TMP_Text>();
            ResetText();
        }
        
        void Update()
        {
            if (shopSingleItem.canSpawn == false)
            {
                if (timeRemaning > 0)
                {
                    timeRemaning -= Time.deltaTime;
                }
                else
                {
                    timeRemaning = 0f;
                }
                textMeshPro.text = name + " build in progress." + "\n\r" + "Time Remaining: " + timeRemaning.ToString("#");
            }
            else
            {
                ResetTimer();
                ResetText();
            }
        }

        void ResetTimer()
        {
            name = shopSingleItem.itemInfo.name;
            description = shopSingleItem.itemInfo.description;
            energyRequired = shopSingleItem.itemInfo.energyRequired;
            buildTime = shopSingleItem.itemInfo.buildTime;
            timeRemaning = buildTime;
        }

        void ResetText()
        {
            textMeshPro.text = "Name: " + name + "\r\n" + "Description: " + description + "\r\n"  + "Energy Cost: " + energyRequired + "\n\r" + "Build Time: " + buildTime;
        }
    }
}
