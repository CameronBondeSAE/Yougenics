using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_ViewModel : MonoBehaviour
{
    public DoorModel door;
    public Animator animator;

    private void Start()
    {
        door.onDoorInteractEvent += PlayAnimation;
    }

    private void PlayAnimation(bool isOpen)
    {
        if(isOpen)
        {
            animator.SetTrigger("OpenDoor");
        }
        else
        {
            animator.SetTrigger("CloseDoor");
        }
    }
}
