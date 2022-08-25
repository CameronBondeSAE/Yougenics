using System.Collections;
using System.Collections.Generic;
using Minh;
using UnityEngine;
using TMPro;

public class ItemNameUI : MonoBehaviour
{
    
    public string name;
    TMP_Text textMeshPro;
    
    // Start is called before the first frame update
    void Start()
    {
        
        textMeshPro = GetComponent<TMP_Text>();
        
            //slot.slot1.GetInfo().name;
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = name;
    }
    
 }
