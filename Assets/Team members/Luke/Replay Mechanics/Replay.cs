using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Replay : MonoBehaviour
{
    public event Action EnableInputEvent;
    public event Action DisableInputEvent;

    public List<Action<float>> inputs = new();
    public List<float> timeStamps = new();

    public bool isRecording = true;

    private PlayerControls _playerControls;
    private InputAction _recordReplay;

    private int i;
    private float replayStartTime;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        timeStamps.Add(0);
    }
    
    private void OnEnable()
    {
        _recordReplay = _playerControls.Player.RecordAndReplay;
        _recordReplay.Enable();
        _recordReplay.performed += ToggleRecordAndReplay;
    }
    
    private void OnDisable()
    {
        _recordReplay.Disable();
        _recordReplay.performed -= ToggleRecordAndReplay;
    }

    private void ToggleRecordAndReplay(InputAction.CallbackContext context)
    {
        if (isRecording)
        {
            DisableInputEvent?.Invoke();
            isRecording = false;
            //Load initial conditions
            i = 0;
            replayStartTime = Time.time;
            StartCoroutine(PlayReplay());
        }
        else
        {
            StopCoroutine(PlayReplay());
            EnableInputEvent?.Invoke();
            inputs.Clear();
            timeStamps.Clear();
            timeStamps.Add(0);
            isRecording = true;
        }
    }

    private IEnumerator PlayReplay()
    {
        yield return new WaitForSeconds(timeStamps[i+1] - timeStamps[i]);
        inputs[i].Invoke(Time.time-replayStartTime);
        float error = Time.time - replayStartTime - timeStamps[i+1];
            Debug.Log("Error = " + error);

        if (++i < timeStamps.Count-1) StartCoroutine(PlayReplay());
    }
}
