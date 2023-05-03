using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyStateBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject.transform.parent.gameObject, stateInfo.length);
    }
}