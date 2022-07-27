using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NodeCanvas.Framework;
using UnityEngine.Serialization;

namespace Alex
{
    public class ButtonViewModel : MonoBehaviour
    {

        public Transform buttonMesh;
        public Button buttonModel;
        public Transform buttonHolder;
        public ParticleSystem particles;
        private Vector3 originalPos;
        


        void Awake()
        {
            originalPos = new Vector3(buttonMesh.transform.position.x, buttonMesh.transform.position.y,buttonMesh.transform.position.z);
        }
        void Start()
        {
            buttonModel.buttonPressedEvent += Press;
        }
        public void Press()
        {
            
            if (buttonModel.canInteract)
            {
                buttonMesh.DOMove(buttonHolder.position, 1);
                particles.Play();
                buttonModel.canInteract = false;
                StartCoroutine(ResetButtonPos());
                //FindObjectOfType<AudioManager>().Play("Button");
            }
        }
        
        public IEnumerator ResetButtonPos()
        {
            while (buttonModel.canInteract == false)
            {
                yield return new WaitForSeconds(2f);
                {
                    //Puts the button back to its starting position, cancels the particle animation and allows the button to be used again
                    
                    buttonMesh.transform.position = originalPos;
                    particles.Stop();
                    buttonModel.canInteract = true;
                }
            }
        }
    }
}
