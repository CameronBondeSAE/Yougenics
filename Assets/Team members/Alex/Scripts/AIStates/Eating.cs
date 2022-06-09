using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Eating : StateBase
    {

        public Eating()
        {
            Debug.Log("Eating");
        }
        
        public override void EatingTest()
        {
            //Overide.DebugLog("Test");
            //base.Test();    
            //Testing();
            base.EatingTest();
            Debug.Log("We're doing an eating test");
        }





    }
}
