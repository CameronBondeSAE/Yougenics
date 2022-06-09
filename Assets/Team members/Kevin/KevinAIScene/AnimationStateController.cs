using System.Collections;
using System.Collections.Generic;
using Kevin;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private FoxModel _foxModel;
    private int energyIsLowHash;
    private int isPatrollingHash;
    private int isChasingHash;
    void Start()
    {
        animator = GetComponent<Animator>();
        _foxModel = GetComponent<FoxModel>();
        energyIsLowHash = Animator.StringToHash("energyIsLow");
        isPatrollingHash = Animator.StringToHash("isPatrolling");
        isChasingHash = Animator.StringToHash("isChasing");
    }
    
    void Update()
    {
        if (_foxModel.energyLow)
        {
            animator.SetBool(energyIsLowHash,true);
        }

        if (_foxModel.energyPoints >= 100f)
        {
            animator.SetBool(energyIsLowHash,false);
        }

        if (_foxModel.isPatrolling)
        {
            animator.SetBool(isPatrollingHash, true);
        }

        if (_foxModel.isPatrolling == false)
        {
            animator.SetBool(isPatrollingHash, false);
        }

        if (_foxModel.isChasing)
        {
            animator.SetBool(isChasingHash, true);
        }
        
        if (_foxModel.isChasing == false)
        {
            animator.SetBool(isChasingHash, false);
        }
    }
}
