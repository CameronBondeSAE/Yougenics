using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Ollie
{
    public class Avoidance : MonoBehaviour
    {
        private Rigidbody rigidbody;
        private RaycastHit hitData;
        private RaycastHit hitDataFloor;
        private Vector3 myPos;
        private Transform parentTransform;
        private int fovAngle;
        public List<GameObject> ignoreList;
        
        void Start()
        {
            rigidbody = GetComponentInParent<Rigidbody>();
            parentTransform = GetComponentInParent<Transform>();
            
            //replace with parent's sight/FOV
            fovAngle = 30;
        }
        
        void FixedUpdate()
        {
            myPos = parentTransform.position;

            for (int i = -90; i < 90; i+=20)
            {
                Vector3 rayAngle = Quaternion.Euler(25, i, 0) * Vector3.forward;
                rayAngle = transform.TransformDirection(rayAngle);
                Ray ray = new Ray(myPos + Vector3.up, rayAngle.normalized);
                //if(i > 0) Debug.DrawRay(ray.origin, ray.direction * hitData.distance, Color.red, 0.1f);
                //if(i < 0) Debug.DrawRay(ray.origin, ray.direction * hitData.distance, Color.blue, 0.1f);
                
                if (Physics.Raycast(ray, out hitDataFloor) && rigidbody.velocity.y < 1)
                {
                    if (hitDataFloor.distance > 4 && i > 0 && hitDataFloor.collider.gameObject.GetComponent<Terrain>())
                    {
                        rigidbody.AddRelativeTorque(0,-2,0);
                        rigidbody.AddRelativeForce(Vector3.back/2);
                        //print("floor check applied turn LEFT");
                    }
                    else if (hitDataFloor.distance > 4 && i < 0 &&
                             hitDataFloor.collider.gameObject.GetComponent<Terrain>())
                    {
                        rigidbody.AddRelativeTorque(0,2,0);
                        rigidbody.AddRelativeForce(Vector3.back/2);
                        //print("floor check applied turn RIGHT");
                    }
                }
            }
            
            for (int i = -fovAngle; i < fovAngle; i+=15)
            {
                Vector3 rayAngle = Quaternion.Euler(0, i, 0) * parentTransform.forward;
                Ray ray = new Ray(myPos, rayAngle.normalized);


                #region Visualize FOV
                // if (i is > -5 and < 5) 
                // {
                //      Debug.DrawRay(ray.origin, ray.direction * hitData.distance, Color.red, 0.01f);
                // }
                // else
                // {
                //      Debug.DrawRay(ray.origin, ray.direction * hitData.distance, Color.blue, 0.01f);
                // }
                #endregion
                
                //to check for terrain slopes
                //Vector3.Angle(Vector3.Up,normal) so it returns the angle between up (y = 90) and the angle of the slope
                //if slope too steep, turn
                //if slope not too steep, keep going
                //can apply the same logic to pathfinding, but shoot a ray down the middle of the OverlapBox and check the angle there
                //then treat that node as blocked if it's too steep
                //can also have another feeler shooting forward and DOWN to check if a slope/cliff/edge is too steep and turn away from it
                
                if (Physics.Raycast(ray, out hitData))
                {
                    if (hitData.distance < 1 && i is > -5 and < 5 && !ignoreList.Contains(hitData.collider.gameObject) && !hitData.collider.gameObject.GetComponent<CritterTrigger>() && !hitData.collider.gameObject.GetComponent<Terrain>())
                    {
                        //emergency bailout left
                        rigidbody.AddRelativeTorque(0,-6,0);
                    }
                    
                        //right rays, turn left
                    else if (hitData.distance < 2 && i > 0 && !ignoreList.Contains(hitData.collider.gameObject) && !hitData.collider.gameObject.GetComponent<CritterTrigger>() && !hitData.collider.gameObject.GetComponent<Terrain>())
                    {
                        rigidbody.AddRelativeTorque(0,-4,0);
                        rigidbody.AddRelativeForce(Vector3.back/2);
                    }
                    
                        //left rays, turn right
                    else if (hitData.distance < 3 && i < 0 && !ignoreList.Contains(hitData.collider.gameObject) && !hitData.collider.gameObject.GetComponent<CritterTrigger>() && !hitData.collider.gameObject.GetComponent<Terrain>())
                    {
                        rigidbody.AddRelativeTorque(0,4,0);
                        rigidbody.AddRelativeForce(Vector3.back/2);
                    }
                    
                    //-((i/fovAngle)*2)
                    
                    //if X distance
                    //addRelativeTorque (0, angle * turnForce, 0)
                    //need to set turnForce to something
                    //CAMMATH
                    //i want -2 for 30 angle
                    //i want +2 for -30 angle
                    //something like angle.normalized, but that's probably generic
                    //actual angle (i) / fovAngle (30) effectively normalises it
                    //eg 30 / 30 = 1.... * turnForce (2) = 2!
                    // -15 / 30 = -0.5 * 2 = - 1
                }
            }
            
            
        }
    }
}
