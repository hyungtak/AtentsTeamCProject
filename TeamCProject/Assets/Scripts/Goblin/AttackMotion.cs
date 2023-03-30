using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackMotion : MonoBehaviour
{

    Animator anim;
    public Action OnAttackEnter;
    public Action OnAttackExit;

    /// <summary>
    /// 공격범위 안에 Player인시 시 AttackAnimation
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            OnAttackEnter?.Invoke();
          
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Monster monster = GetComponentInParent<Monster>();
    //        if (monster != null)
    //        {
    //            monster.Attack();
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
         if (other.CompareTag("Player"))
        {
            OnAttackExit?.Invoke();
        }

    }



}

