using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace John
{
    public class PlayerCameraModel : MonoBehaviour
    {
        public bool        lockMouse;
        public PlayerModel target;
        public float       heightOffset;

        float xRotation;

        private void Start()
        {
            if (lockMouse)
                Cursor.lockState = CursorLockMode.Locked;

            //Wanted to set players camera to their active camera? - not working
            //Camera.SetupCurrent(GetComponent<Camera>());

            //transform.SetParent(target.transform);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = target.gameObject.transform.position;

            //Setting x rotation to match mouse direction + locking it at 90 angle
            xRotation -= target.mouseY * target.lookSensitivity * Time.deltaTime;
            xRotation =  Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
    }
}