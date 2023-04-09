using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAttack3 : MonoBehaviour
{
    public Action OnBoss3Enter;
    //public Action OnAttackStay;
    public Action OnBoss3Exit;

    /// <summary>
    /// 공격범위 안에 Player인시 시 AttackAnimation
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            OnBoss3Enter?.Invoke();
          
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        OnAttackStay?.Invoke();
            
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
         if (other.CompareTag("Player"))
        {
            OnBoss3Exit?.Invoke();
        }

    }



}

