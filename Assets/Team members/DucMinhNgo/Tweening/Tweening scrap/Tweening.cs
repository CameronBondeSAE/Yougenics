using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweening : MonoBehaviour
{

     public AnimationCurve Curve1;
     public AnimationCurve Curve2;
     public AnimationCurve Curve3;
     private float timer;
     
    
     public Transform thingToMessWith;
    
     public void Update()
     { timer += Time.deltaTime;
           // Read values from the AnimationCurve in the inspector
           float animatedValue = Curve1.Evaluate(timer);
           float cur2 = Curve2.Evaluate(timer);
           float cur3 = Curve3.Evaluate(timer);
    // Simple scale for example
           thingToMessWith.localScale = new Vector3(animatedValue, cur2, cur3); 
           if (timer >= Curve1.length) 
           {
               // Loop
               timer = 0;
           }
           if (timer >= Curve2.length) 
           {
               // Loop
               timer = 0;
           }
           if (timer >= Curve3.length) 
           {
               // Loop
               timer = 0;
           }
     }

}
