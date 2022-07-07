using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

namespace Alex
{
    public class ButtonViewModel : MonoBehaviour
    {

        public Transform buttonMesh;
        public Transform resetPos;
        public Button buttonModel;
        public Transform buttonHolder;
        public ParticleSystem particles;
        


        void Start()
        {
            buttonModel.buttonPressedEvent += Press;
        }
        // Start is called before the first frame update
        public void Press()
        {
            
            if (buttonModel.canInteract)
            {
                buttonMesh.DOMove(buttonHolder.position, 1);
                particles.Play();
                buttonModel.canInteract = false;
                StartCoroutine(ResetButtonPos());
            }
        }
        
        public IEnumerator ResetButtonPos()
        {
            while (buttonModel.canInteract == false)
            {
                yield return new WaitForSeconds(2f);
                {
                    buttonMesh.position = resetPos.position;
                    particles.Stop();
                    buttonModel.canInteract = true;
                }
            }
        }
    }
}
