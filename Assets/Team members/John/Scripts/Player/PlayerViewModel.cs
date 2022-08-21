using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;

namespace John
{
    public class PlayerViewModel : NetworkBehaviour
    {
        public PlayerModel playerModel;
        public TMP_Text    nameTagText;
        ClientInfo         client;
        public Animator animator;
        public AudioSource audioSource;
        public AudioClip deathSound;
        public AudioClip normalJumpSound;
        public AudioClip sprintJumpSound;
        AudioClip finalJumpSound;

        private void Start()
        {
            //playerModel.onClientAssignedEvent += InitClientInfo;

            if (playerModel.myClientInfo != null)
            {
                playerModel.myClientInfo.onNameChangeEvent += UpdateNameTag;
                nameTagText.text                           =  playerModel.myClientInfo.ClientName.Value.ToString();
            }
            else
            {
                Debug.Log("Cannot update NameTag, No Client Reference Found");
            }

            playerModel.onJumpEvent += PlayJumpAnimation;
            playerModel.onDeathEvent += PlayDeathAnimation;
            playerModel.onSprintEvent += OnPlayerSprint;
            playerModel.onMovementEvent += PlayMovementDirectionAnimation;
            playerModel.onMovementSpeedEvent += PlayMovementAnimation;

            //Setting default jump sound
            finalJumpSound = normalJumpSound;
        }

        private void OnPlayerSprint(bool isSprinting)
        {
            //Update Jump Sounds
            if (isSprinting)
                finalJumpSound = sprintJumpSound;
            else
                finalJumpSound = normalJumpSound;
        }

        private void PlayMovementAnimation(float speed)
        {
            animator.SetFloat("Speed", speed);
        }

        private void PlayMovementDirectionAnimation(Vector2 movement)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        private void PlayDeathAnimation()
        {
            animator.SetTrigger("Death");

            //Play SFX
            audioSource.clip = deathSound;
            audioSource.Play();
        }

        private void PlayJumpAnimation()
        {
            animator.SetTrigger("Jump");

            //Play SFX
            audioSource.clip = finalJumpSound;
            audioSource.Play();
        }

        /*private void InitClientInfo()
{
   client = playerModel.myClientInfo;
   client.onNameChangeEvent += UpdateNameTag;
   nameTagText.text = client.ClientName.Value.ToString();
}*/

        private void UpdateNameTag(string newName)
        {
            nameTagText.text = newName;
        }
    }
}