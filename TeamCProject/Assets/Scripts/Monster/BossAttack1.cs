using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAttack1 : MonoBehaviour
{
    public Action OnBoss1Enter;
    public Action OnBoss1Stay;
    public Action OnBoss1Exit;

    /// <summary>
    /// 공격범위 안에 Player인시 시 AttackAnimation
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            OnBoss1Enter?.Invoke();
          
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnBoss1Stay?.Invoke();

        }
    }

    private void OnTriggerExit(Collider other)
    {
         if (other.CompareTag("Player"))
        {
            OnBoss1Exit?.Invoke(); 
        }

    }



}

