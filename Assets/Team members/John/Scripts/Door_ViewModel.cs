using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_ViewModel : MonoBehaviour
{
    public DoorModel door;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    private void Start()
    {
        door.onDoorInteractEvent += PlayAnimation;
    }

    private void PlayAnimation(bool isOpen)
    {
        if(isOpen)
        {
            animator.SetTrigger("OpenDoor");

            //SFX
            audioSource.clip = openDoorSound;
            audioSource.Play();
        }
        else
        {
            animator.SetTrigger("CloseDoor");

            audioSource.clip = closeDoorSound;
            audioSource.Play();
        }
    }
}
