using System;
using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using Unity.Netcode;
using UnityEngine;

namespace Ollie
{
    public class StateViewer : NetworkBehaviour
    {
        //public StateDictionary stateDictionary;
        
        private ParticleSystemRenderer particleSystem;
        private AudioSource[] sounds;
        private AudioSource deathSound;
        private AudioSource eatingSound;
        private AudioSource matingSound;
        private AudioSource sleepingSound;
        private AudioSource walkingSound;
        public List<Material> particleMaterials;
        private CritterAIPlanner parent;

        private AudioClip audioClip;
        private Material particleMaterial;
        private String text;
        
        //Particle Materials
        //0 - aggressive
        //1 - gotofood
        //2 - hungry
        //3 - mating
        //4 - sleep
        //5 - wander
        //6 - dead
        //7 - eating

        private void Start()
        {
            particleSystem = GetComponent<ParticleSystemRenderer>();
            sounds = GetComponents<AudioSource>();
            deathSound = sounds[0];
            eatingSound = sounds[1];
            matingSound = sounds[2];
            sleepingSound = sounds[3];
            walkingSound = sounds[4];
            parent = GetComponentInParent<CritterAIPlanner>();
            //stateDictionary = this.gameObject.GetComponentInChildren<StateDictionary>();
        }

        /*public void ChangeViewInfo(AntAIState aiState)
        {
            if (aiState != null)
            {
                audioClip = stateDictionary.aiStateInfo[aiState.GetType()].audioClip;
                particleMaterial = stateDictionary.aiStateInfo[aiState.GetType()].particleMaterial;
                text = stateDictionary.aiStateInfo[aiState.GetType()].text;
            }
            

            if (IsClient)
            {
                if (audioClip != null)
                {
                    //AudioSource.PlayClipAtPoint(audioClip,parent.transform.position);
                }
                if (particleMaterial != null) particleSystem.material = particleMaterial;
            }
        }*/
        
        public void ChangeParticles(int index)
        {
            
            if (IsClient)
            {
                particleSystem.material = particleMaterials[index];
            }
            
            if (index == 6 && !deathSound.isPlaying) deathSound.Play();
            if (index == 5 && !walkingSound.isPlaying) walkingSound.Play();
            if (index == 3 && !matingSound.isPlaying) matingSound.Play();
            if (index == 4 && !sleepingSound.isPlaying) sleepingSound.Play();
            if (index == 7 && !eatingSound.isPlaying) eatingSound.Play();
        }
    }
}
