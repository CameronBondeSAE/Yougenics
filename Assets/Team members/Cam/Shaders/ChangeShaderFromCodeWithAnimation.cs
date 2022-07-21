using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShaderFromCodeWithAnimation : MonoBehaviour
{
    public Animator animator;
    
    [Button]
    public void SuperSpike()
    {
        animator.SetTrigger("SpikeAngry");
    }

    public void DamageEnable()
    {
        Debug.Log("Damage = "); //+enable);
    }
    public void DamageDisable()
    {
        Debug.Log("Damage dis "); //+enable);
    }
}
