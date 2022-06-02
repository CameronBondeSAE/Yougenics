using System.Collections;
using System.Collections.Generic;
using Kevin;
using UnityEngine;

namespace Kevin
{
    public class PlayerEntityTest : MonoBehaviour, IEntity
    {
        /*public void OnTriggerEnter(Collider other)
        {
            IEntity entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                entity.InRange = true;
                transform.localScale = new Vector3(5f, 5f, 5f);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            IEntity entity = other.GetComponent<IEntity>();
            if (entity != null)
            {
                entity.InRange = false; 
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }*/

        public bool InRange { get; set; }
    }
}

