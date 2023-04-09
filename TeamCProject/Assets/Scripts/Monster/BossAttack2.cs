using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAttack2 : MonoBehaviour
{
    public Action OnBoss2Enter;
    //public Action OnBoss2Stay;
    public Action OnBoss2Exit;

    /// <summary>
    /// 공격범위 안에 Player인시 시 AttackAnimation
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            OnBoss2Enter?.Invoke();
          
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
            OnBoss2Exit?.Invoke();
        }

    }



}

