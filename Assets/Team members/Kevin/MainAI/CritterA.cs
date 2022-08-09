using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CritterA : CreatureBase, IEdible
    {
        
        //List of Entities
        public List<Transform> predatorList;
        public List<Transform> mateList;
        public List<Transform> preyList;
        
        
        public void OnEnable()
        {
            maxAge = 0f;
            gestationTime = 0f;
            litterSizeMax = 0;
            metabolism = 0f;
            mutationRate = 0f;
            empathy = 0f;
            aggression = 0f;
            dangerLevel = 0f;
            size = 0f;
            colour = new Color(Random.Range(0f, 10f),Random.Range(0f, 10f),Random.Range(0f, 10f)); 
        }

        public void OnDisable()
        {
            
        }

        #region ListManager

        public void Profiler(Collider other)
        {
            CritterA otherCritter = other.GetComponent<CritterA>();
            if (otherCritter != null)
            {
                Debug.Log("Profiled!");
                //if(otherCritter.dangerLevel > dangerLevel)
            }
        }
        
        public void VisionExit(Collider other)
        {
            ListRemover(other.transform);
        }
        
        public void ListRemover(Transform _transform)
        {
            if (predatorList.Contains(_transform))
            {
                predatorList.Remove(_transform);
            }
            if (mateList.Contains(_transform))
            {
                mateList.Remove(_transform);
            }
            if (preyList.Contains(_transform))
            {
                preyList.Remove(_transform);
            }
        }

        #endregion

        #region AI Movement

        private void TurnTo(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime);
        }

        #endregion

        #region IEdible

        public float GetEnergyAmount()
        {
            throw new NotImplementedException();
        }

        public float EatMe(float energyRemoved)
        {
            throw new NotImplementedException();
        }

        #endregion
     
    }


