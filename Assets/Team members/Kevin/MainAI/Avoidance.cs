using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Kevin
{
    public class Avoidance : MonoBehaviour
    {
        private Rigidbody rb;
        public float turnForce;
        [SerializeField] private float rayDistance;

        [SerializeField] private Vector3 dirR;
        [SerializeField] private Vector3 dirL;

        public bool rays;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            Vector3 dirF = transform.forward;
            /*Vector3 dirR = Quaternion.Euler(0, 30f, 0) * transform.forward;
            Vector3 dirL = Quaternion.Euler(0, -30f, 0) * transform.forward;*/

            RaycastHit hitInfo;
            RaycastHit hitRight;
            RaycastHit hitLeft;

            for (int i = -30; i < 30f; i++)
            {
                Vector3 dir = Quaternion.Euler(0, i, 0) * transform.forward;
                if (rays)
                {
                    if (i <= -20)
                    {   
                        Debug.DrawRay(transform.position, dir * rayDistance, Color.green);
                    }
                    
                    if (i >= 20)
                    {  
                        Debug.DrawRay(transform.position, dir * rayDistance, Color.green);
                    }
                    
                    if (i >= -19 && i <= -10)
                    {    
                        Debug.DrawRay(transform.position, dir * rayDistance, Color.blue);
                    }
                    
                    if (i <= 19 && i >= 10)
                    {    
                        Debug.DrawRay(transform.position, dir * rayDistance, Color.blue);
                    }
                    
                    if (i >= -9 && i <= -1)
                    {     
                        Debug.DrawRay(transform.position, dir * rayDistance, Color.red);
                    }
                    
                    if (i <= 9 && i >= 0)
                    { 
                        Debug.DrawRay(transform.position, dir * rayDistance, Color.red);
                    }
                }

                if (i <= -20)
                {
                    if (Physics.Raycast(new Ray(transform.position, dir), out hitInfo, rayDistance, 255,
                            QueryTriggerInteraction.Ignore))
                    {
                        //Debug.DrawRay(transform.position, dir * rayDistance, Color.green);
                        rb.AddRelativeForce(Vector3.back / 6f);
                        rb.AddRelativeTorque(0, turnForce * 1.25f, 0, ForceMode.Impulse);
                    }
                }

                if (i >= 20)
                {
                    if (Physics.Raycast(new Ray(transform.position, dir), out hitInfo, rayDistance, 255,
                            QueryTriggerInteraction.Ignore))
                    {
                        //Debug.DrawRay(transform.position, dir * rayDistance, Color.green);
                        rb.AddRelativeForce(Vector3.back / 6f);
                        rb.AddRelativeTorque(0, turnForce * 1.25f, 0, ForceMode.Impulse);
                    }
                }

                if (i >= -19 && i <= -10)
                {
                    if (Physics.Raycast(new Ray(transform.position, dir), out hitInfo, rayDistance / 2.5f, 255,
                            QueryTriggerInteraction.Ignore))
                    {
                        //Debug.DrawRay(transform.position, dir * rayDistance, Color.blue);
                        rb.AddRelativeForce(Vector3.back / 4f);
                        rb.AddRelativeTorque(0, turnForce * 2f, 0, ForceMode.Impulse);
                    }
                }

                if (i <= 19 && i >= 10)
                {
                    if (Physics.Raycast(new Ray(transform.position, dir), out hitInfo, rayDistance / 2.5f, 255,
                            QueryTriggerInteraction.Ignore))
                    {
                        //Debug.DrawRay(transform.position, dir * rayDistance, Color.blue);
                        rb.AddRelativeForce(Vector3.back / 4f);
                        rb.AddRelativeTorque(0, turnForce * 2f, 0, ForceMode.Impulse);
                    }
                }

                if (i >= -9 && i <= -1)
                {
                    if (Physics.Raycast(new Ray(transform.position, dir), out hitInfo, rayDistance, 255,
                            QueryTriggerInteraction.Ignore))
                    {
                        //Debug.DrawRay(transform.position, dir * rayDistance, Color.red);
                        rb.AddRelativeForce(Vector3.back / 2f);
                        rb.AddRelativeTorque(0, turnForce, 0, ForceMode.Impulse);
                    }
                }

                if (i <= 9 && i >= 1)
                {
                    if (Physics.Raycast(new Ray(transform.position, dir), out hitInfo, rayDistance, 255,
                                QueryTriggerInteraction.Ignore))
                    {
                        //Debug.DrawRay(transform.position, dir * rayDistance, Color.red);
                        rb.AddRelativeForce(Vector3.back / 2f);
                        rb.AddRelativeTorque(0, turnForce, 0, ForceMode.Impulse);
                    }
                }
                
                if (i == 0)
                {
                    if (Physics.Raycast(new Ray(transform.position, dir), out hitInfo, rayDistance/rayDistance, 255,
                                QueryTriggerInteraction.Ignore))
                    {
                        rb.AddRelativeForce(Vector3.back / 2f);
                        rb.AddRelativeTorque(0, turnForce, 0, ForceMode.Impulse);
                    }
                    Debug.DrawRay(transform.position, dir * rayDistance, Color.yellow);
                }

                if (Physics.Raycast(new Ray(transform.position, Vector3.forward)) && rayDistance < 2f)
                {
                    rb.AddRelativeTorque(0, turnForce, 0, ForceMode.Impulse);
                }

                if (Physics.Raycast(new Ray(transform.position, Vector3.forward), out hitInfo, rayDistance, 255,
                        QueryTriggerInteraction.Collide) && hitInfo.transform.gameObject.GetComponent<CritterB>())
                {
                    rb.AddRelativeForce(Vector3.back);
                    
                }
            }
        }
    }
}

