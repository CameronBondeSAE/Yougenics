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
      void Start()
      {
         button.buttonPressedEvent += DestroyThis;
      }

      public void DestroyThis()
      {
         Destroy(gameObject);
      }

   }
}
