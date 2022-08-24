using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ollie
{
    public class StateViewer : MonoBehaviour
    {
        private ParticleSystemRenderer particleSystem;
        private AudioSource[] sounds;
        private AudioSource deathSound;
        private AudioSource eatingSound;
        private AudioSource matingSound;
        private AudioSource sleepingSound;
        private AudioSource walkingSound;
        public List<Material> particleMaterials;
        private CritterAIPlanner parent;
        
        //Particle Materials
        //0 - aggressive
        //1 - gotofood
        //2 - hungry
        //3 - mating
        //4 - sleep
        //5 - wander
        //6 - dead

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
        }

        public void ChangeParticles(int index)
        {
            //if isServer or isClient
            particleSystem.material = particleMaterials[index];
            if (index == 6 && !deathSound.isPlaying) deathSound.Play();
            if (index == 5 && !walkingSound.isPlaying) walkingSound.Play();
            if (index == 3 && !matingSound.isPlaying) matingSound.Play();
            if (index == 4 && !sleepingSound.isPlaying) sleepingSound.Play();
            if (index == 2 && !eatingSound.isPlaying) eatingSound.Play();

            //need an extra state for actually eating, and line that up with eat sound
        }
    }
}
