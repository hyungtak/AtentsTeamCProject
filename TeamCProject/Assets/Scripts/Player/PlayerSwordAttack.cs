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
            Golem golemrHealth = other.GetComponent<Golem>();
            if (golemrHealth != null)
            {
                golemrHealth.MonsterTakeDamage(swordAttackDamage);
            }

            Wizard wizardHealth = other.GetComponent<Wizard>();
            if (wizardHealth != null)
            {
                wizardHealth.MonsterTakeDamage(swordAttackDamage);
            }
            
            Goblin monsterHealth = other.GetComponent<Goblin>();
            if(monsterHealth != null)
            {
                monsterHealth.MonsterTakeDamage(swordAttackDamage);
            }


        }
    }

    ///현재 문제점
    ///몬스터 몸에 많은 양의 충돌체(Collider)가

}
