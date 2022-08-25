using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using TMPro;

public class ItemNameUI : MonoBehaviour
{
    
    public string name;
    TMP_Text textMeshPro;
    ItemSlot itemSlot;
    public bool isItem1;
    public bool isItem2;
    
    // Start is called before the first frame update
    
    
    public void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        itemSlot = GetComponentInParent<ItemSlot>();
        itemSlot.item1PickedUpEvent += UpdateItem1Active;
        itemSlot.item2PickedUpEvent += UpdateItem2Active;
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = name;
    }
    
    public void UpdateItem1Active(bool isActive)
    {
        if(isItem1)
            if (isActive)
                gameObject.SetActive(true);

            else if (isItem1)
                gameObject.SetActive(false);
    }
        
    public void UpdateItem2Active(bool isActive)
    {
        if(isItem2)
            if (isActive)
                gameObject.SetActive(true);

            else if (isItem2)
                gameObject.SetActive(false);
    }
    
 }
