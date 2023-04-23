using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat : MonoBehaviour
{
    /// <summary>
    /// 최대Hp
    /// </summary>
    public int monsterMaxHp = 10;

    /// <summary>
    /// 현재 Hp
    /// </summary>
    int currentMonsterHp = 10;

    private float speed;
    public float moveRange = 0.01f;

    /// <summary>
    /// 애니메이션 변환값or속도값
    /// </summary>
    private int move;

    /// <summary>
    /// 공격을 했는지 안했는지 true 했다 , false 안했다.
    /// </summary>
    private bool AttackCheck = false;

    /// <summary>
    /// 몬스터가 기본 회전값
    /// </summary>
    const int transRotate = 180;

    private float gN = 0.3f;

    /// <summary>
    /// 플레이어 위치 저장 할 변수
    /// </summary>
    Transform playerTrans;
    Player player;

    Rigidbody rigid;
    Animator anim;

    Transform ground;
    /// <summary>
    /// coin 오브젝트 받기
    /// </summary>
    public GameObject coin;

    /// <summary>
    /// 공격 실패시 돌아갈 장소
    /// </summary>
    Vector3 startPosition;

    /// <summary>
    /// 플레이어 트리거 인식 false 인식x true 인식o
    /// </summary>
    bool find = false;

    private void Awake()
    {
        currentMonsterHp = monsterMaxHp;
        //필요한 Component 가져오기
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        player = GetComponent<Player>();
        Detect detect = GetComponentInChildren<Detect>();
        if (detect != null)
        {
            detect.OnEnter += OnDetectPlayerEnter;
            detect.OnExit += OnDetectPlayerExit;
        }
        else
        {
            Debug.LogError("Detect 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        ground = GameObject.FindGameObjectWithTag("Ground").transform;
        Player.playerDied += OnPlayerDied;

        startPosition = transform.position;

        //코루틴 실행
        StartCoroutine(transMovement());
    }

    private void FixedUpdate()
    {
        MonsterMove();
    }

    /// <summary>
    /// 이벤트 등록 해제
    /// </summary>
    void OnDestroy()
    {
        Player.playerDied -= OnPlayerDied;
    }

    /// <summary>
    /// 플레이어 죽었을 시
    /// </summary>
    void OnPlayerDied()
    {
        find = false;
        anim.SetBool("PlayerDie", true);
        move = 0;
        //StartCoroutine(transMovement());
    }

    //몬스터 감지 델리게이트--------------------------------------------------
    private void OnDetectPlayerEnter()
    {
        StopAllCoroutines();
        Vector3 playerPos = playerTrans.position - transform.position;
        playerPos.y -= 0.3f;
        transform.rotation = Quaternion.LookRotation(playerPos);

        AttackCheck = true;
        StartCoroutine(playerDetect());
    }

    private void OnDetectPlayerExit()
    {
        anim.SetBool("Detect", false);
        find = false;

        //다시 자동 이동
        StartCoroutine(transMovement());
    }


    


    /// <summary>
    /// 몬스터 움직임
    /// </summary>
    private void MonsterMove()
    {

        //플레이어를 인식 했을 때
        if (find)
        {
            if (playerTrans != null)
            {
                speed = 4f;
                StopAllCoroutines();
                transform.position += transform.forward * speed * Time.fixedDeltaTime;
                float distance = Vector3.Distance(transform.position, ground.position);
                if (distance < gN)
                {
                    transform.position = startPosition;
                    // 일정 거리 이내에 도달한 경우 실행할 코드 작성
                }
              
            }

            else if (playerTrans == null)
            {
                rigid.MovePosition(transform.position + Time.fixedDeltaTime * 0 * transform.forward);
                //anim.SetBool("Jump", true);
            }
        }
        //인식 안했을 때 행동                              
        else
        {
            speed = 1f;
            float BatY = Mathf.Sin(Time.time * speed) * moveRange;
            Vector3 newPosition = transform.position + Time.fixedDeltaTime * move * transform.forward;
            newPosition.y = BatY + 1f;
            rigid.MovePosition(newPosition);

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
            move = UnityEngine.Random.Range(0, 2);
            anim.SetInteger("Move", move);

            if (move != 0)
            {
                transform.Rotate(0, transRotate, 0);  //좌우 회전
            }

            //3초 마다
            yield return new WaitForSeconds(5f);

        }

    }

    IEnumerator playerDetect()
    {
        move = 0;
        anim.SetBool("Detect", true);
        yield return null;
        //AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        //float currentTime = stateInfo.normalizedTime * stateInfo.length;
        //yield return new WaitForSeconds(currentTime);
        //anim.SetBool("Attack", true);
        //find = true;

    }

    void Cheking()
    {
        anim.SetBool("Attack", true);
        find = true;
    }




    public void MonsterTakeDamage(int damageAmount)
    {
        currentMonsterHp -= damageAmount;
        anim.SetTrigger("Damage");

        AttackCheck = false;
        move = 0;
        Invoke("HitAnim", 1f);

        if (currentMonsterHp <= 0)
        {
            MonsterDie();
        }
   
    }

    private void MonsterDie()
    {
        //죽었을 시 사망 애니메이션 실행 예정
        anim.SetTrigger("Dead");
        Destroy(gameObject, 0.4f);

        //코인 위치 설정 후 생성
        Vector3 center = transform.position;
        center.y = 0.5f;

        GameObject obj = Instantiate(coin, center, Quaternion.identity);

    }


    void HitAnim()
    {
        anim.SetBool("Hit", false);
        move = 1;

        AttackCheck = true;
    }
}
