using System;
using System.Collections;
using System.Collections.Generic;
using Cam;
using UnityEngine;
using Random = UnityEngine.Random;
using Minh;
using UnityEngine.Rendering;

public class CritterB : CreatureBase, IEdible
    {
        public float acceleration;
        public float visionRadius = 8f;
        public float visionCenterZ = 2.5f;
        public float myDangerLevel;
        public float rayDistance;
        public Rigidbody rb;
        
        //Separate Components
        [SerializeField] private CommonAttributes commonAttributes;
        public Energy energy;
        public Health health;
        
        //List of Entities
        public List<Transform> predatorList;
        public List<Transform> mateList;
        public List<Transform> foodList;
        public Transform closestPredator;
        public Transform closestMate;
        public Transform closestFood;

        public bool isWandering;
        public bool isRunning;
        public bool isHunting;
        public bool isEating;
        public bool isHorny;
        public bool isMating;
        public bool isDead;
        public bool isSleeping ;
        public bool isInDanger;
        public bool foundFood;
        public bool isHungry;
        public bool caughtFood;
        public bool foundMate;
        
        //view variables 
        public float alpha;
        public float invisCooldown = 10f;
        public bool canInvis = true;
        public Renderer renderer;
        public GameObject noseObject;
        public Renderer noseRenderer;
        public override void Awake()
        {
            commonAttributes = GetComponent<CommonAttributes>();
            energy = GetComponent<Energy>();
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
            renderer = GetComponent<Renderer>();
            noseRenderer = noseObject.GetComponent<Renderer>();
        }
        
        public void OnEnable()
        {
            //Stats
            maxAge = 0f;
            gestationTime = 0f;
            litterSizeMax = 0;
            metabolism = 0f;
            mutationRate = 0f;
            empathy = 0f;
            aggression = 0f;
            sizeScale = 0f;
            colour = new Color(Random.Range(0f, 10f),Random.Range(0f, 10f),Random.Range(0f, 10f));
            sex = (Sex) Random.Range(0, 1);
            commonAttributes.dangerLevel =  Random.Range(0, 10);
            myDangerLevel = commonAttributes.dangerLevel;
        }

        public void OnDisable()
        {
            //On Death maybe
        }

        public void Profiler(Collider other)
        {
            CreatureBase otherCreatureBase = other.GetComponent<CreatureBase>();
            IEdible otherEdible = other.GetComponent<IEdible>();
            CommonAttributes otherCommonAttributes = other.GetComponent<CommonAttributes>();
            
            RaycastHit hitInfo;
            //Physics.Raycast(transform.position,other.transform.position, out hitInfo, other.transform.position.magnitude - transform.position.magnitude,255,QueryTriggerInteraction.Ignore)
            if (otherCreatureBase != null || otherEdible != null)
            {
                Debug.DrawRay(transform.position,Vector3.forward,Color.red);
                //Predator List
                if (otherCreatureBase != null && sex == otherCreatureBase.sex && myDangerLevel < otherCommonAttributes.dangerLevel)
                {
                    predatorList.Add(other.transform);
                }
                
                //Mate List
                if (otherCreatureBase != null && otherCreatureBase.sex != sex)
                {
                    mateList.Add(other.transform);
                }
                
            
                //Food List
                if (otherEdible != null && myDangerLevel > otherCommonAttributes.dangerLevel) 
                {
                    foodList.Add(other.transform);
                }
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
            if (foodList.Contains(_transform))
            {
                foodList.Remove(_transform);
            }
        }

        #region Special Functions

        public void Chameleon(float alpha)
        {
            renderer.material.SetFloat("_Alpha", alpha);
            noseRenderer.material.SetFloat("_Alpha", alpha);
            StartCoroutine(Visible());
        }

        public void CooldownFunction()
        {
            StartCoroutine(InvisibleCooldown());
        }

        #endregion

        #region IEnumerators

        public IEnumerator SleepTimer()
        {
            yield return new WaitForSeconds(10f);
            energy.EnergyAmount.Value = 100f;
            isSleeping = false;
        }

        public IEnumerator Visible()
        {
            yield return new WaitForSeconds(5f);
            Chameleon(1f);
        }

        public IEnumerator InvisibleCooldown()
        {
            yield return new WaitForSeconds(invisCooldown);
            canInvis = true;
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

        #region ISense Bools

        public bool IsHungry()
        {
            if (energy.EnergyAmount.Value < 25f && !isEating)
            {
                isHungry = true;
            }
            else
            {
                isHungry = false;
            }

            return isHungry;
        }

        public bool FoundFood()
        {
            if (foodList.Count > 0 && !isEating) 
            {
                foundFood = true;
            }
            else
            {
                foundFood = false;
            }

            return foundFood;
        }

        public bool IsHunting()
        {
            if (!isSleeping && isHungry && foundFood && !isEating)
            {
                isHunting = true;
            }
            else
            {
                isHunting = false;
            }

            return isHunting;
        }

        public bool CaughtFood()
        {
            if (caughtFood)
            {
                caughtFood = true;
            }
            else
            {
                caughtFood = false;
            }

            return caughtFood;
        }

        public bool IsEating()
        {
            if (isEating == true && foundFood == true)
            {
                isEating = true;
            }
            else
            {
                isEating = false;
            }

            return isEating;
        }

        public bool IsInDanger()
        {
            if (predatorList.Count > 0)
            {
                isInDanger = true;
            }
            else
            {
                isInDanger = false;
            }
            return isInDanger;
        }

        public bool RunAway()
        {
            if (isInDanger && !isSleeping)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

            return isRunning;
        }

        public bool IsSleeping()
        {
            if (energy.EnergyAmount.Value < 1)
            {
                isSleeping = true;
            }
            else
            {
                isSleeping = false;
            }

            return isSleeping;
        }
        

        #endregion
    }

