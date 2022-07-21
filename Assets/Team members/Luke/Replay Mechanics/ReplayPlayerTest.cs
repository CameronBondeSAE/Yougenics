using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ReplayPlayerTest : MonoBehaviour
{
    private PlayerControls _playerControls;
    private InputAction _action1;
    private InputAction _action2;

    [FormerlySerializedAs("_replay")] [SerializeField] private Replay replay;

    void Awake()
    {
        _playerControls = new PlayerControls();
    }
    
    void OnEnable()
    {
        replay.EnableInputEvent += EnableInputs;
        replay.DisableInputEvent += DisableInputs;
        
        _action1 = _playerControls.Player.Action1;
        _action1.Enable();
        _action1.performed += DoAction1;

        _action2 = _playerControls.Player.Action2;
        _action2.Enable();
        _action2.performed += DoAction2;
    }
    
    void OnDisable()
    {
        replay.EnableInputEvent -= EnableInputs;
        replay.DisableInputEvent -= DisableInputs;
        
        _action1.Disable();
        _action1.performed -= DoAction1;

        _action2.Disable();
        _action2.performed -= DoAction2;
    }
    
    private void EnableInputs()
    {
        _action1.Enable();
        _action2.Enable();
    }

    private void DisableInputs()
    {
        _action1.Disable();
        _action2.Disable();
    }

    private void DoAction1(InputAction.CallbackContext context)
    {
        Action1(Time.time);
    }

    private void Action1(float time)
    {
        if (replay.isRecording)
        {
            replay.inputs.Add(Action1);
            replay.timeStamps.Add(Time.time);
        }
        Debug.Log("Action1 " + time);
    }

    private void DoAction2(InputAction.CallbackContext context)
    {
        Action2(Time.time);
    }
    
    private void Action2(float time)
    {
        if (replay.isRecording)
        {
            replay.inputs.Add(Action2);
            replay.timeStamps.Add(Time.time);
        }
        Debug.Log("Action2 " + time);
    }
}
