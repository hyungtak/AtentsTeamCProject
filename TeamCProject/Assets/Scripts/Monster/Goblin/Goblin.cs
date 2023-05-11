using System.Collections;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class Goblin : MonsterBase
{
    /// <summary>
    /// 공격을 했는지 안했는지 true 실행이 완료되었다 , false 실행 중이다.
    /// </summary>
    private bool AttackCheck = false;

    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        AttackMotion attack = GetComponentInChildren<AttackMotion>();

        if (attack != null)
        {
            attack.OnAttackEnter += OnAttackEnter;
            attack.OnAttackExit += OnAttackExit;
        }
        else
        {
            Debug.LogError("Detect 컴포넌트를 찾을 수 없습니다.");
        }
    }


    //공격 실행--------------------------------------------------------------
    private void OnAttackEnter()
    {
        animator.SetBool("Attacko", true);
        AttackCheck = false;
        move = 0;
        StopAllCoroutines();
    }

    private void OnAttackExit()
    {
        animator.SetBool("Attacko", false);
        AttackCheck = true;
        move = 1;
    }

    //몬스터 감지-------------------------------------------------------------------
    protected override void OnDetectPlayerEnter()
    {
        base.OnDetectPlayerEnter();
        move = 1;
        animator.SetBool("Run", true);
    }

    protected override void OnDetectPlayerStay()
    {
        base.OnDetectPlayerStay();

        if (AttackCheck)
        {
            move = 1;
            animator.SetBool("Run", true);
        }

    }
    protected override void OnDetectPlayerExit()
    {
        animator.SetBool("Run", false);
        base.OnDetectPlayerExit();
    }


    protected override void MonsterMove()
    {
        base.MonsterMove();
    }


    public void MonsterTakeDamage(int damageAmount)
    {
        currentMonsterHp -= damageAmount;
        animator.SetBool("Hit", true);           //이벤트 함수 사용
        AttackCheck = false;
        move = 0;

        if (currentMonsterHp <= 0)
        {
            MonsterDie();
        }
    }

    private void MonsterDie()
    {

        animator.SetTrigger("Dead");
        Destroy(gameObject, 0.4f);

        Vector3 center = transform.position;
        center.y = 0.5f;
        _ = Instantiate(coin, center, Quaternion.identity);

    }


    void HitAnim()
    {
        animator.SetBool("Hit", false);
        move = 1;
        AttackCheck = true;
    }


}
