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
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            Gizmos.color = Color.white;
         
            Vector3 monsterSenter = transform.position;
            monsterSenter.y = 23f;

            Gizmos.DrawWireSphere(monsterSenter/20, sphereCollider.radius/20);
        }



    }
}

