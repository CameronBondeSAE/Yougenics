using System.Collections;
using System.Collections.Generic;
using Kevin;
using TMPro;
using UnityEngine;

public class TimeRemaining : MonoBehaviour

{
    private TMP_Text textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //EnergyLevel();
        textMeshPro.text = "Time Remaining: " + GetComponent<GameManager>().remainingTime.ToString("0:00:00"); 
    }
}
