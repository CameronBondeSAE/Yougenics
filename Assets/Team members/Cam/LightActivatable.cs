using Cam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cam
{
    public class LightActivatable : MonoBehaviour
    {
        public Light  light;
        public Button button;

        // Start is called before the first frame update
        void Start()
        {
            button.PressedEvent += ButtonOnPressedEvent;
        }

        void ButtonOnPressedEvent()
        {
            light.enabled = true;
        }
    }

}