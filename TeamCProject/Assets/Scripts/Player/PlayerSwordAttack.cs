using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{
    public int swordAttackDamage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            //Monster 컴포넌트 가져옴
            Monster monsterHealth = other.GetComponent<Monster>();
            if (monsterHealth != null)
            {
                monsterHealth.MonsterTakeDamage(swordAttackDamage);
            }
        }
    }

    ///현재 문제점
    ///몬스터 몸에 많은 양의 충돌체(Collider)가

}
