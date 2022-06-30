using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{


    public class Vision : MonoBehaviour
    {
        public List<Food> foodIveSeen;
        public List<CritterModel> potentialMatesIveSeen;

        private void OnTriggerEnter(Collider other)
        {
            Food onePiece = other.gameObject.GetComponent<Food>();
            if (other.CompareTag("Food") && !foodIveSeen.Contains(onePiece))
            {
                foodIveSeen.Add(onePiece);
            }
            CritterModel oneOfUs = other.gameObject.GetComponent<CritterModel>();
            if (other.CompareTag("MayasCritter") && !potentialMatesIveSeen.Contains(oneOfUs))
            {
                if(other.GetComponent<Horny>() != null && other.GetComponent<Horny>().currentHorny >= 75)
                    potentialMatesIveSeen.Add(oneOfUs);
            }
        }
    }
}
