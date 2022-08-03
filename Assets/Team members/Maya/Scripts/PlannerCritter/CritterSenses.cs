using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using Minh;
using UnityEngine;

namespace Maya
{ 
    public class CritterSenses : MonoBehaviour, ISense
    {
        public Touch touch;
        public Vision vision;
        public Horny myHorny;
        public Energy myEnergy;
        public Health myHealth;
        private Transform myPos; //where i am
        private Vector3 matePos; //where my potential mate is
        private Vector3 foodPos; //where food is
        private Vector3 enemyPos; //where enemies are
        void Awake()
        {
            myPos = GetComponent<Transform>();
        }
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(CritterAI.isHealthy, myHealth.Hp > 40);
            aWorldState.Set(CritterAI.isHorny, myHorny.currentHorny > 75);
            aWorldState.Set(CritterAI.isHungry, myEnergy.EnergyAmount.Value < 65);
            aWorldState.Set(CritterAI.isTired, myEnergy.EnergyAmount.Value < 25);
            aWorldState.Set(CritterAI.canSeeEnemy, vision.potentialVictimsIveSeen.Count > 0);
            aWorldState.Set(CritterAI.canSeeFood, vision.whereFoodIs.Count >= 0);
            aWorldState.Set(CritterAI.canSeeMate, vision.potentialMatesIveSeen.Count > 0);
            aWorldState.Set(CritterAI.isNearEnemy, touch.isNearVictim);
            aWorldState.Set(CritterAI.isNearFood, touch.isNearFood);
            aWorldState.Set(CritterAI.isNearMate, touch.isNearMate);
            aWorldState.Set(CritterAI.mateIsHorny, touch.myMatesHornyToo);
            
        }
    }
}
