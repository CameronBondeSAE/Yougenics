using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{
    public class EnergyGlowShader : MonoBehaviour
    {
        public CritterSenses critterMe;
        private Material shader;
        // Start is called before the first frame update
        void Start()
        {
            shader = GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            shader.SetFloat("_GlowByEnergy", critterMe.myEnergy.EnergyAmount.Value);
        }
    }
}
