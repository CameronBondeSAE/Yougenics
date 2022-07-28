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
        string nameTxt;
        string descriptionTxt;
        float energyRequiredTxt;
        float buildTimeTxt;
        float timeRemaning;
        float startTimeTxt;
        

        TMP_Text textMeshPro;

        void Start()
        {
            ResetTimer();

            textMeshPro = GetComponent<TMP_Text>();

            ResetText();

        }

        // Update is called once per frame
        void Update()
        {
            if (shopSingleItem.beingBuilt)
            {
                if (timeRemaning > 0)
                {
                    timeRemaning -= Time.deltaTime;
                }
                else
                {
                    timeRemaning = 0f;
                }
                textMeshPro.text = nameTxt + " build in progress." + "\n\r" + "Time Remaining: " + timeRemaning.ToString("#");
            }
            else
            {
                ResetTimer();
                ResetText();
            }
        }

        void ResetTimer()
        {
            nameTxt = shopSingleItem.itemInfo.name;
            descriptionTxt = shopSingleItem.itemInfo.description;
            energyRequiredTxt = shopSingleItem.itemInfo.energyRequired;
            buildTimeTxt = shopSingleItem.itemInfo.buildTime;
            timeRemaning = buildTimeTxt;
        }

        void ResetText()
        {
            textMeshPro.text = "Name: " + nameTxt + "\r\n" + "Description: " + descriptionTxt + "\r\n"  + "Energy Cost: " + energyRequiredTxt + "\n\r" + "Build Time: " + buildTimeTxt;
        }
    }
}
