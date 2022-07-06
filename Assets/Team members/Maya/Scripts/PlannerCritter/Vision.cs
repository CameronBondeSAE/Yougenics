using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{


    public class Vision : MonoBehaviour
    {
        public List<IEdible> foodIveSeen;
        public List<Transform> whereFoodIs;
        
        public List<Horny> potentialMatesIveSeen;
        
        
        
        public Vision(List<IEdible> foodIveSeen)
        {
            this.foodIveSeen = foodIveSeen;
        }

        private void OnTriggerEnter(Collider other)
        {
            IEdible onePiece = other.gameObject.GetComponent<IEdible>();
            if (other.GetComponent<IEdible>() != null && !foodIveSeen.Contains(onePiece))
            {
                foodIveSeen.Add(onePiece);
                Transform onePieceTransform = ((Component) GetComponent<IEdible>()).transform;
                whereFoodIs.Add(onePieceTransform);

            }
            Horny oneOfUs = other.gameObject.GetComponent<Horny>();
            if (other.CompareTag("MayasCritter") && !potentialMatesIveSeen.Contains(oneOfUs))
            {
                if(other.GetComponent<Horny>() != null && other.GetComponent<Horny>().currentHorny >= 75)
                    potentialMatesIveSeen.Add(oneOfUs);
            }
        }
    }
}
