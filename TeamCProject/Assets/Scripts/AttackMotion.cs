using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMotion : MonoBehaviour
{

    /// <summary>
    /// 공격범위 안에 Player인시 시 AttackAnimation
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Monster monster = GetComponentInParent<Monster>();
            if (monster != null)
            {

                monster.Attack();
            }
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

    //private void OnTriggerExit(Collider other)
    //{
    //    Monster monster = GetComponentInParent<Monster>();
    //    if (monster != null)
    //    {
    //        monster.StopAttack();
    //    }

    //}


    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;

        position.y += 2.2f;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(position,3.2f);

    }
   


}

