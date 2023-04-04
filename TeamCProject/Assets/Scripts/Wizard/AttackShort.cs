using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShort : StateMachineBehaviour
{
    Wizard wizard;

    private void Awake()
    {
        wizard = FindObjectOfType<Wizard>();
    }




    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log($"StateExit : {wizard.gameObject.name}");
        wizard.FireStart();
    }

}
