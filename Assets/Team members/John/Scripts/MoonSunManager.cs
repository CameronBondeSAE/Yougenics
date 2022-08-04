using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSunManager : MonoBehaviour
{
    public GameObject sun, moon;

    [Header("Reference Only")]
    [SerializeField]
    float sunPos;
    [SerializeField]
    float currentTime;

    private void Start()
    {
        DayNightManager.instance.PhaseChangeEvent += UpdateSunState;
    }

    private void UpdateSunState(DayNightManager.DayPhase phase)
    {
        if(phase == DayNightManager.DayPhase.Evening)
        {
            sun.SetActive(false);
            moon.SetActive(true);
        }
        else if(phase == DayNightManager.DayPhase.Dawn)
        {
            sun.SetActive(true);
            moon.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = DayNightManager.instance.CurrentTime.Value;
        sunPos = currentTime * 15.0f - 180.0f;
        transform.rotation = Quaternion.Euler(sunPos, 0, 0);
    }
}
