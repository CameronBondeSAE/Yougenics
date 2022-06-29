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
        public CritterModel critterModel;
        private Transform myPos; //where i am
        private Vector3 matePos; //where my potential mate is
        private Vector3 foodPos; //where food is
        private Vector3 enemyPos; //where enemies are
        public bool isAlsoHorny; //placeholder
        void Awake()
        {
            myPos = GetComponent<Transform>();
        }
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(CritterAI.isHealthy, myHealth.Hp < 40);
            aWorldState.Set(CritterAI.isHorny, false);
            aWorldState.Set(CritterAI.isHungry, myEnergy.energyAmount < 60);
            aWorldState.Set(CritterAI.isTired, myEnergy.energyAmount <= 20);
            aWorldState.Set(CritterAI.canSeeEnemy, false);
            aWorldState.Set(CritterAI.canSeeFood, vision.foodIveSeen.Count > 0);
            aWorldState.Set(CritterAI.canSeeMate, false);
            aWorldState.Set(CritterAI.isNearEnemy, false);
            aWorldState.Set(CritterAI.isNearFood, touch.isNearFood);
            aWorldState.Set(CritterAI.isNearMate, false);
            aWorldState.Set(CritterAI.mateIsHorny, false);
            
        }
    }
}
