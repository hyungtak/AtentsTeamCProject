using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Wizard : MonsterBase
{

    /// <summary>
    /// 파이어볼
    /// </summary>
    public GameObject fireBall;

    bool onPlayer = false;


    WaitForSeconds StartDelay = new WaitForSeconds(1);

    Transform fireTransform;

    Animator animator;
    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();

        fireTransform = transform.GetChild(0);
    }


    protected override void OnDetectPlayerEnter()
    {
        move = 0;
        onPlayer = true;
        base.OnDetectPlayerEnter();
        StartCoroutine(FireballDelay());
    }

    protected override void OnDetectPlayerExit()
    {
        move = 1;

        onPlayer = false;
        animator.SetBool("Attack", false);

    }


    IEnumerator FireballDelay()
    {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
    }

    protected override IEnumerator TransMovement()
    {
        return base.TransMovement();
    }

    public void FireStart()
    {
        GameObject obj = Instantiate(fireBall);
        obj.transform.position = fireTransform.position;
    }

    /// <summary>
    /// 몬스터 데미지 받다
    /// </summary>
    /// <param name="damageAmount"></param>
    public void MonsterTakeDamage(int damageAmount)
    {
        currentMonsterHp -= damageAmount;
        animator.SetBool("Damage", true);

        if (currentMonsterHp <= 0)
        {
            MonsterDie();
        }
    }
    private void MonsterDie()
    {
        StopAllCoroutines();
        animator.SetTrigger("End");
        Destroy(gameObject, 1.5f);

        Vector3 center = transform.position;
        center.y = 1.5f;

        GameObject obj = Instantiate(coin, center, Quaternion.identity);
        //죽었을 시 사망 애니메이션 실행 예정
    }

    public void DamageFalse()
    {
        animator.SetBool("Damage", false);
    }

    /// <summary>
    /// 파이어볼 애니메이션 끝날 때 까지 플레이어쪽 보기
    /// </summary>
    void FireBallCheck()
    {
        if (onPlayer)
        {
            StartCoroutine(FireballDelay());
        }
        else
        {
            transform.LookAt(monsterTransform);
            find = false;
            StartCoroutine(TransMovement());
        }
    }
}
