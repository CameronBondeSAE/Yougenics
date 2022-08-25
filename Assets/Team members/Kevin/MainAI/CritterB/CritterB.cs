using System;
using System.Collections;
using System.Collections.Generic;
using Cam;
using Kevin;
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
        public Minh.Health health;
        
        //List of Entities
        public List<Transform> entityList;
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
        public bool canRun;
        
        //view variables 
        public float alpha;
        public float deathColour = 1f;
        public float invisCooldown = 10f;
        public bool canInvis = true;
        public Renderer renderer;
        public GameObject noseObject;
        public Renderer noseRenderer;
        public GameObject deathOrb;
        public TurnTowards turnTowards;
        public GameObject feetObject;
        public GameObject feet2Object;
        public Renderer feetRenderer;
        public Renderer feet2Renderer;
        
        //Audio
        public AudioClip myAudioClip;
        public AudioClip snarlClip;
        public AudioClip eatingClip;
        public AudioClip animalScreech;
        public AudioSource audioClip;

        public FoodChain myFoodChain;        
        public enum FoodChain 
        {
            Predator,
            Prey,
            Neutral
        }
        
        
        public override void Awake()
        {
            //Critter Stats
            commonAttributes = GetComponent<CommonAttributes>();
            energy = GetComponent<Energy>();
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
            
            //Renderers
            renderer = GetComponent<Renderer>();
            noseRenderer = noseObject.GetComponent<Renderer>();
            feetRenderer = feetObject.GetComponent<Renderer>();
            feet2Renderer = feet2Object.GetComponent<Renderer>();
            
            turnTowards = GetComponent<TurnTowards>();

            myFoodChain = (FoodChain) Random.Range(0, 3);
            myAudioClip = GetComponent<AudioClip>();
            audioClip = GetComponent<AudioSource>(); 
        }
        
        public void OnEnable()
        {
            CritterStats();
        }

        public void CritterStats()
        {
            age = 0f;
            ageOfMatingStart = 50f;
            ageOfMatingEnd = 250f;
            maxAge = Random.Range(150f, 300f);
            gestationTime = 25f;
            litterSizeMax = Random.Range(1, 5);
            metabolism = 15f;
            mutationRate = Random.Range(1f, 25f);
            sizeScale = 0f;
            maxSize = 1f;
            sex = (Sex) Random.Range(0, 1);
            commonAttributes.dangerLevel =  Random.Range(0, 10);
            myDangerLevel = commonAttributes.dangerLevel;
            
            //empathy = Random.Range(1f, 25f);
            //aggression = Random.Range(1f, 25f);
            //colour = new Color(Random.Range(0f, 10f),Random.Range(0f, 10f),Random.Range(0f, 10f));
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
            CritterB otherCritterB = other.GetComponent<CritterB>(); //This was getting triggered when running into all AI's not just AI's with critterB causing null errors
            RaycastHit hitInfo;
            
            
            //Physics.Raycast(transform.position,other.transform.position, out hitInfo, other.transform.position.magnitude - transform.position.magnitude,255,QueryTriggerInteraction.Ignore)
            if ((otherCreatureBase != null || otherEdible != null) && otherCritterB != null)
            {
                //if(Physics.Raycast(transform.position,other.transform.position, out hitInfo, 10f,255,QueryTriggerInteraction.Ignore))
                //{
                    //Debug.DrawRay(transform.position,Vector3.forward,Color.red);
                    
                    //Predator List
                    /*if (otherCreatureBase != null && sex == otherCreatureBase.sex && myDangerLevel < otherCommonAttributes.dangerLevel)
                    {
                        predatorList.Add(other.transform);
                    }*/

                    if (otherCreatureBase != null && otherCritterB.myFoodChain == FoodChain.Predator)
                    {
                        predatorList.Add(other.transform);
                    }
                    else if (otherCritterB.myFoodChain == FoodChain.Predator && sex == Sex.Male && otherCritterB.sex == Sex.Female)
                    {
                        entityList.Add(other.transform);
                        mateList.Add(other.transform);
                    }
                    else if (myFoodChain == FoodChain.Predator && myDangerLevel < otherCommonAttributes.dangerLevel) 
                    {
                        predatorList.Add(other.transform);
                    }
                    
                    //Mate List
                    if (otherCreatureBase != null && sex == Sex.Male && otherCritterB.sex == Sex.Female && myFoodChain == otherCritterB.myFoodChain)
                    {
                        entityList.Add(other.transform);
                        mateList.Add(other.transform);
                    }
                    
                    //Neutral List 
                    if (myFoodChain == FoodChain.Neutral)
                    {
                        entityList.Add(other.transform);
                        mateList.Add(other.transform);
                    }

                    //Food List
                    if (otherEdible != null)
                    {
                        //normal food that doesnt have common attributes component
                        if (otherCommonAttributes == null)
                        {
                            foodList.Add(other.transform);
                        }
                        else if (myFoodChain == FoodChain.Predator && myDangerLevel > otherCommonAttributes.dangerLevel) 
                        {
                            foodList.Add(other.transform);
                        }
                        
                    }
                //}
            
            }
        }

        public void VisionExit(Collider other)
        {
            ListRemover(other.transform);
        }
        
        public void ListRemover(Transform _transform)
        {
            if (entityList.Contains(_transform))
            {
                entityList.Remove(_transform);
            }
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

        #region View Functions

        public void Chameleon(float alpha)
        {
            renderer.material.SetFloat("_Alpha", alpha);
            noseRenderer.material.SetFloat("_Alpha", alpha);
            feetRenderer.material.SetFloat("_Alpha", alpha);
            feet2Renderer.material.SetFloat("_Alpha", alpha);
            StartCoroutine(Visible());
        }

        public void Dead()
        {
            renderer.material.SetFloat("_Color", deathColour);
            noseRenderer.material.SetFloat("_Color", deathColour);
            deathOrb.SetActive(true);
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
            GetComponent<Wander>().enabled = true;
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

        public bool IsDead()
        {
            if (health.CurrentHealth.Value < 1)
            {
                isDead = true;
            }
            else
            {
                isDead = false;
            }

            return isDead;
        }

        #endregion
    }

