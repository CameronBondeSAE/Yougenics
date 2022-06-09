using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    // Set the value of each phase to the time it should start
    // NOTE: Does not work with non-whole-hour times (e.g. 7:30)
    public enum DayPhase
    {
        Midnight = 0,
        Dawn = 5,
        Morning = 7,
        Noon = 12,
        Evening = 17,
        Night = 21
    }

    [Tooltip("Set this to the starting state, it will overwrite the time and call the phase's event")]
    public DayPhase currentPhase = DayPhase.Morning;

    [Tooltip("Measured as hours in 24h format, i.e. value of 17f = 5pm")]
    public float currentTime = 7f;

    [Tooltip("How many real-time seconds it takes for 1 in-game hour")]
    public float timeDilation = 10f;

    /// <summary>
    /// Use if statement to check what phase has just started
    /// E.g. "if phase == DayNightManager.DayPhase.Morning" for morning functions
    /// </summary>
    public event Action<DayPhase> PhaseChangeEvent;

    private void Start()
    {
        ChangePhase(currentPhase);
    }

    private void Update()
    {
        // Check to ensure no divide by zero errors
        if (timeDilation > 0)
        {
            currentTime += Time.deltaTime / timeDilation;
        }

        // Wraps time back to 0 when it hits midnight
        if (currentTime >= 24f)
        {
            ChangePhase(DayPhase.Midnight);
        }

        foreach (DayPhase phase in Enum.GetValues(typeof(DayPhase)))
        {
            // Checks if time has passed the phase time and that phase is next in the sequence
            // More modular than a bunch of if statements, just add a phase to the enum and this will include it
            if (currentTime >= (float)phase && (int)phase > (int)currentPhase)
            {
                ChangePhase(phase);
            }
        }
    }

    public void ChangePhase(DayPhase newPhase)
    {
        currentPhase = newPhase;
        currentTime = (float)newPhase;
        PhaseChangeEvent?.Invoke(newPhase);
        print(newPhase);
    }
}
