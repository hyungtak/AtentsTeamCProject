
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Monster : AttackSign
{
    //추가 할 것
    //몬스터 Hp int
    public int monsterHp = 10;

    //몬스터 Attack  or Damage 설정
    public int monsterDamage = 1;

    //수정

    //플레이어 인식 시 플레이어쪽으로 회전.//완료

    Rigidbody rigid;
    Animator anim;

    /// <summary>
    /// 몬스터가 기본 회전값
    /// </summary>
    const int transRotate = 180;

    /// <summary>
    /// 애니메이션 변환값or속도값
    /// </summary>
    private int move;
    

    /// <summary>
    /// 플레이어 트리거 인식
    /// </summary>
    bool find = false;

    private Vector3 monsterTransform;


    protected void Awake()
    {

        //필요한 Component 가져오기
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }



    protected override void Start()
    {
        
        transform.Rotate(0, 90, 0);
        base.Start();
        //코루틴 실행
        StartCoroutine(transMovement());
    }


    //FixedUpdate는 물리 시뮬레이션 갱신 주기에 맞춰서 호출된다. 
    private void FixedUpdate()
    {
        //몬스터움직임 함수
        MonsterMove();
    }

    /// <summary>
    /// 플레이어가 트리거에 접촉했다
    /// </summary>
    /// <param name="other">"Player"</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();

            find = true;
            anim.SetBool("Run", true);
        }

    }

    //플레이어가 트리거 안에 지속적으로 인식
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterTransform = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(monsterTransform);
            find = true;
            anim.SetBool("Run", true);
            //anim.SetBool("AttackB",true);
        }
    }


    /// <summary>
    /// 트리거 밖에 플레이어가 나갔을 때
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        find = false;

        if (other.CompareTag("Player"))
        {
            //Run애니메이션 >> Walk로
            anim.SetBool("Run", false);
            anim.SetBool("AttackB", false);
            //다시 자동 이동
            StartCoroutine(transMovement());
        }

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
            move = Random.Range(0, 3);
            anim.SetInteger("Walk", move);

            if (move != 0)
            {
                transform.Rotate(0, transRotate, 0);  //좌우 회전
            }

            //3초 마다
            yield return new WaitForSeconds(3f);

        }     

    }


    /// <summary>
    /// 몬스터 움직임
    /// </summary>
    private void MonsterMove()
    {
        
        //플레이어를 인식 했을 때 
        if(find)
        {
            StopAllCoroutines();
            move = 3;
            //dir이 플레이어 방향 찾고 크기는 1 
            Vector3 dir = (player.position - transform.position).normalized;

            //몬스터 위치 + 속도 * DetaTime* 플레이 방향 
            rigid.MovePosition(transform.position + move * Time.fixedDeltaTime * dir);
        }

     //인식 안했을 때 행동                              
     else
            rigid.MovePosition(transform.position + Time.fixedDeltaTime * move * transform.forward);


    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(30, 10, 2));
    }


}
