using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DayNightManager : NetworkBehaviour
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
    //public DayPhase currentPhase = DayPhase.Morning;
    public NetworkVariable<DayPhase> CurrentPhase = new NetworkVariable<DayPhase>();

    [Tooltip("Measured as hours in 24h format, i.e. value of 17f = 5pm")]
    public NetworkVariable<float> CurrentTime = new NetworkVariable<float>();

    [Tooltip("How many real-time seconds it takes for 1 in-game hour")]
    public float timeDilation = 10f;
    //public NetworkVariable<float> TimeDilation = new NetworkVariable<float>();

    /// <summary>
    /// Use if statement to check what phase has just started
    /// E.g. "if phase == DayNightManager.DayPhase.Morning" for morning functions
    /// </summary>
    public event Action<DayPhase> PhaseChangeEvent;

    private void Start()
    {
        if(IsServer)
            ChangePhase(CurrentPhase.Value);
    }

    private void Update()
    {
        if (!IsServer)
            return;

        // Check to ensure no divide by zero errors
        if (timeDilation > 0)
        {
            CurrentTime.Value += Time.deltaTime / timeDilation;
        }

        // Wraps time back to 0 when it hits midnight
        if (CurrentTime.Value >= 24f)
        {
            ChangePhase(DayPhase.Midnight);
        }

        foreach (DayPhase phase in Enum.GetValues(typeof(DayPhase)))
        {
            // Checks if time has passed the phase time and that phase is next in the sequence
            // More modular than a bunch of if statements, just add a phase to the enum and this will include it
            if (CurrentTime.Value >= (float)phase && (int)phase > (int)CurrentPhase.Value)
            {
                ChangePhase(phase);
            }
        }
    }

    public void ChangePhase(DayPhase newPhase)
    {
        CurrentPhase.Value = newPhase;
        CurrentTime.Value = (float)newPhase;
        PhaseChangeEvent?.Invoke(newPhase);
        print(newPhase);
    }
}
