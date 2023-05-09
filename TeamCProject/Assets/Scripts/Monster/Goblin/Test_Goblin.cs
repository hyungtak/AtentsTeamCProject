using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Goblin : MonsterBase
{
    /// <summary>
    /// 공격을 했는지 안했는지 true 했다 , false 안했다.
    /// </summary>
    private bool AttackCheck = false;

    Animator anim;

    const int transRotate = 180;

    /// <summary>
    /// 애니메이션 변환값or속도값
    /// </summary>
    private int move;


    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
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

    /// <summary>
    /// 플레이어가 죽었을 때
    /// </summary>
    protected override void OnPlayerDied(int _)
    {
        base.OnPlayerDied(0);
        anim.SetBool("Jump", true);
    }

    //공격 실행--------------------------------------------------------------
    private void OnAttackEnter()
    {
        anim.SetBool("Attacko", true);
        AttackCheck = false;
        move = 0;
        StopAllCoroutines();
    }

    private void OnAttackExit()
    {
        anim.SetBool("Attacko", false);
        AttackCheck = true;
        move = 1;
    }

    //몬스터 감지-------------------------------------------------------------------
    protected override void OnDetectPlayerEnter()
    {
        base.OnDetectPlayerEnter();
        move = 1;
        anim.SetBool("Run", true);
    }

    protected override void OnDetectPlayerStay()
    {
        base.OnDetectPlayerStay();

        if (AttackCheck)
        {
            move = 0;
            anim.SetBool("Run", true);
        }
    
    }

    protected override void OnDetectPlayerExit()
    {
        base.OnDetectPlayerExit();
        anim.SetBool("Run", false);
        StartCoroutine(transMovement());
    }

    /// <summary>
    /// 자동 이동 시 몬스터 회전값 설정
    /// </summary>
    /// <returns></returns>
    IEnumerator transMovement()
    {
        while (true)
        {
            //move = 0 == Idle or move != 0 == Walk 실행
            move = UnityEngine.Random.Range(0, 2);

            //animation이름 바꾸기 전부 Walk로

            if (move != 0)
            {
                transform.Rotate(0, transRotate, 0);  //좌우 회전
            }
            //3초 마다
            yield return new WaitForSeconds(5f);
        }
    }
    protected override void MonsterMove()
    {
        base.MonsterMove();
    }



    public override void MonsterTakeDamage(int damageAmount)
    {
        base.MonsterTakeDamage(damageAmount);

        anim.SetBool("Hit", true);           //이벤트 함수 사용
        AttackCheck = false;
        move = 0;

     
        Invoke("HitAnim", 1f);

    }

    protected override void MonsterDie()
    {
        anim.SetTrigger("Dead");
        Destroy(gameObject, 0.4f);
        
        base.MonsterDie();
    }


    void HitAnim()
    {
        anim.SetBool("Hit", false);
        move = 1;
        AttackCheck = true;
    }
}
