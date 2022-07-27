using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Alex
{

    public class ItemInfoUI : MonoBehaviour
    {
        public  GameObject itemToSpawn;
        public ItemInfo itemInterface;
        string name = "";
        string description = "";
        float cost = 0f;

        TMP_Text text;

        // Start is called before the first frame update
        void Start()
        {
            itemInterface = itemToSpawn.GetComponent<IItem>().GetInfo();

            name = itemInterface.name;
            description = itemInterface.description;
            cost = itemInterface.energyRequired;
            text = GetComponent<TMP_Text>();

            text.text = "Name: " + name + "\r\n" + "Description: " + description + "\r\n"  + "Energy Cost: " + cost;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
