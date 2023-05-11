using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonsterBase
{
    /// <summary>
    /// 골렘의 공격패턴변환
    /// </summary>
    int AttackMotion;

    const int transRotate = 180;

    Animator animator;

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponentInChildren<Animator>();

        ////공격 범위 신호
        BossAttack1 boss1 = GetComponentInChildren<BossAttack1>();
        boss1.OnBoss1Enter += OnAttack1Enter;
        boss1.OnBoss1Exit += OnAttack1Exit;

        BossAttack2 boss2 = GetComponentInChildren<BossAttack2>();
        boss2.OnBoss2Enter += OnAttack2Enter;
        boss2.OnBoss2Exit += OnAttack2Exit;
    }


    //플레이어 감지 했을 때----------------------------------------------------------------------------------------------
    /// <summary>
    /// 감지됐다
    /// </summary>
    protected override void OnDetectPlayerEnter()
    {
        base.OnDetectPlayerEnter();
        move = 1;
        animator.SetInteger("Move", move);
    }

    /// <summary>
    /// 감지 중.
    /// </summary>
    protected override void OnDetectPlayerStay()
    {
        base.OnDetectPlayerStay();
        animator.SetInteger("Move", move);
    }

    /// <summary>
    /// 감지범위에서 나갔다.
    /// </summary>
    protected override void OnDetectPlayerExit()
    {
        base.OnDetectPlayerExit();
    }

    //플레이어와 보스에 거리마다 공격 패턴이 달라짐(범위가 긴 순서 1,2)---------------------------------------------------------------------------------------------
    private void OnAttack1Enter()
    {
        StopAllCoroutines();
        AttackMotion = 1;
        move = 0;
        animator.SetInteger("Attack", AttackMotion);


    }

    private void OnAttack1Exit()
    {
        AttackMotion = 0;
        animator.SetInteger("Attack", AttackMotion);
        find = true;
    }

    private void OnAttack2Enter()
    {

        AttackMotion = 2;
        move = 0;
        animator.SetInteger("Attack", AttackMotion);
        StopAllCoroutines();
    }

    private void OnAttack2Exit()
    {
        AttackMotion = 1;
        move = 0;
        animator.SetInteger("Attack", AttackMotion);
        StopAllCoroutines();
    }


    protected override IEnumerator TransMovement()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            //move = 0 == Idle or move != 0 == Walk 실행
            move = UnityEngine.Random.Range(0, 2);

            animator.SetInteger("Move", move);

            if (move != 0)
            {
                transform.Rotate(0, transRotate, 0);  //좌우 회전
            }

            //5초 마다
            yield return new WaitForSeconds(5f);

        }
    }

    protected override void MonsterMove()
    {
        base.MonsterMove();
    }

    //플레이어가 맵에 등장---------------------------------------------------------------------------

    /// <summary>
    /// 플레이어 죽은 신호 받고 실행
    /// </summary>
    protected override void OnPlayerDied(int _)
    {
        find = false;
        move = 0;
        animator.SetBool("PlayerDead", true);
    }


    /// <summary>
    /// 몬스터 데미지 받다
    /// </summary>
    /// <param name="damageAmount"></param>
    public void MonsterTakeDamage(int damageAmount)
    {
        currentMonsterHp -= damageAmount;

        if (currentMonsterHp <= 0)
        {
            MonsterDie();
        }
    }

    /// <summary>
    /// 사망
    /// </summary>
    private void MonsterDie()
    {
        StopAllCoroutines();
        animator.SetTrigger("Die");
        Destroy(gameObject, 1.5f);


        Vector3 center = transform.position;
        center.y = 0.5f;

        _ = Instantiate(coin, center, Quaternion.identity);
    }
}
