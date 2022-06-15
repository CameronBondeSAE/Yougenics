using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Maya
{ 
    public class CritterSenses : MonoBehaviour, ISense
    {
        public Touch touch;
        public Vision vision;
        public CritterModel critterModel;
        private Transform myPos; //where i am
        private Transform matePos; //where my potential mate is
        private Transform foodPos; //where food is
        private Transform enemyPos; //where enemies are
        public bool isAlsoHorny; //placeholder
        void Awake()
        {
            myPos = GetComponent<Transform>();
        }
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(CritterAI.isHealthy, critterModel.myHealth < 40);
            aWorldState.Set(CritterAI.isHorny, critterModel.myHorny < 75);
            aWorldState.Set(CritterAI.isHungry, critterModel.myEnergy < 60);
            aWorldState.Set(CritterAI.isTired, critterModel.myEnergy < 25);
            aWorldState.Set(CritterAI.canSeeEnemy, vision.myTarget = enemyPos);
            aWorldState.Set(CritterAI.canSeeFood, vision.myTarget = foodPos);
            aWorldState.Set(CritterAI.canSeeMate, vision.myTarget = matePos);
            aWorldState.Set(CritterAI.isNearEnemy, touch.myTarget = enemyPos);
            aWorldState.Set(CritterAI.isNearFood, touch.myTarget = foodPos);
            aWorldState.Set(CritterAI.isNearMate, touch.myTarget = matePos);
            aWorldState.Set(CritterAI.mateIsHorny, isAlsoHorny);
        }
    }
}
