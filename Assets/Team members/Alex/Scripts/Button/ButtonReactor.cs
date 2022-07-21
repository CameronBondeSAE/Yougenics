using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alex;
using System;

namespace Alex
{
   public class ButtonReactor : MonoBehaviour
   {
      
      public Button button;
      public Button button1;
      void Start()
      {
         button1.buttonPressedEvent += DestroyThis;
         button.buttonPressedEvent += DestroyThis;
      }

      private void OnDisable()
      {
         button1.buttonPressedEvent -= DestroyThis;
         button.buttonPressedEvent -= DestroyThis;
      }

      public void DestroyThis()
      {
         if (gameObject != null)
         {
            Destroy(gameObject);
         }
      }

   }
}
