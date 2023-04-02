using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    //추가 할 것
    //몬스터 Hp int
    //public int monsterHp = 10;

    //몬스터 Attack  or Damage 설정
    //public int monsterDamage = 1;

    //public delegate void playerDied(bool find);

    Rigidbody rigid;
    Animator anim;

    /// <summary>
    /// 플레이어 위치 저장 할 변수
    /// </summary>
    Transform playerTrans;

    /// <summary>
    /// 몬스터가 기본 회전값
    /// </summary>
    const int transRotate = 180;

    /// <summary>
    /// 애니메이션 변환값or속도값
    /// </summary>
    private int move;

    /// <summary>
    /// 플레이어 트리거 인식 false 인식x true 인식o
    /// </summary>
    bool find = false;

    private Vector3 monsterTransform;

    private void Awake()
    {

        //필요한 Component 가져오기
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        WizardDetect detect = FindObjectOfType<WizardDetect>();
        if (detect != null)
        {
            detect.WizardOnEnter += OnDetectPlayerEnter;
            detect.WizardOnStay += OnDetectPlayerStay;
            detect.WizardOnExit += OnDetectPlayerExit;
        }
        else
        {
            Debug.LogError("Detect 컴포넌트를 찾을 수 없습니다.");
        }


    }



    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        Player.playerDied += OnPlayerDied;

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
    /// 공격애니메이션 실행
    /// </summary>
    public void Attack()
    {
        anim.SetBool("Attack",true);
    }

    /// <summary>
    /// 이벤트 등록 해제
    /// </summary>
    void OnDestroy()
    {
        Player.playerDied -= OnPlayerDied;

    }

    /// <summary>
    /// 신호 받고 몬스터 실행
    /// </summary>
    void OnPlayerDied()
    {
        find = false;
        StartCoroutine(transMovement());
    }


    private void OnDetectPlayerEnter()
    {
        //move = 1;
        StopAllCoroutines();
        find = true;
        anim.SetBool("Attack", true);
    }

    private void OnDetectPlayerStay()
    {
        move = 1;
        monsterTransform = new Vector3(playerTrans.position.x, transform.position.y, playerTrans.position.z);
        transform.LookAt(monsterTransform);
        find = true;
        anim.SetBool("Attack", true);
    }

    private void OnDetectPlayerExit()
    {
        //Run애니메이션 >> Walk로
        move = 0;
        anim.SetInteger("forward", move);
        find = false;
        //다시 자동 이동
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
            //move = 0이면 Idle or move != 0이면 Walk 실행
            move = Random.Range(0, 3);
            anim.SetInteger("forward", move);

            if (move != 0)
            {
                transform.Rotate(0, transRotate, 0);  //좌우 회전
            }

            //3초 마다
            yield return new WaitForSeconds(5f);

        }

    }


    /// <summary>
    /// 몬스터 움직임
    /// </summary>
    private void MonsterMove()
    {
        if (find)
        {
            if (playerTrans != null)
            {
                StopAllCoroutines();
                move = 3;
                //dir이 플레이어 방향 찾고 크기는 1 
                Vector3 dir = (playerTrans.position - transform.position).normalized;
                //몬스터 위치 + 속도 * DetaTime* 플레이 방향 
                rigid.MovePosition(transform.position + 0* Time.fixedDeltaTime * dir);
            }
            else if (playerTrans == null)
            {
                rigid.MovePosition(transform.position + Time.fixedDeltaTime * 0 * transform.forward);
                //anim.SetBool("Run", true);
            }
        }

        //인식 안했을 때 행동                              
        else
            rigid.MovePosition(transform.position + Time.fixedDeltaTime * (move * 0.5f) * transform.forward);








    }
}
