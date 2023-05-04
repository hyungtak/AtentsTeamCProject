using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    //몬스터 Hp int
    public int monsterMaxHp = 10;

    public GameObject coinBox;

    int currentMonsterHp = 100;

    public int monsterDamage = 1;


    /// <summary>
    /// 몬스터가 기본 회전값
    /// </summary>
    const int transRotate = 180;

    /// <summary>
    /// 플레이어를 감지했을 때 이동속도
    /// </summary>
    float maxTimer = 2f;

    /// <summary>
    /// 애니메이션 속도값
    /// </summary>
    private int move;

    private bool isHit = false;

    private float timer = 0.5f;

    int AttackMotion;

    /// <summary>
    /// 플레이어 트리거 인식x = false 인식o = true 
    /// </summary>
    bool find = false;

    private Vector3 monsterTransform;

    /// <summary>
    /// 플레이어 위치 저장 할 변수
    /// </summary>
    Transform playerTrans;

    Rigidbody rigid;
    Animator anim;
    Player player;

    Renderer bossColor;

    protected virtual void Awake()
    {
        currentMonsterHp = monsterMaxHp;
        
        //필요한 Component 가져오기
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
        
        bossColor = GetComponentInChildren<Renderer>();

        //플레이어 감지 신호
        Detect detect = GetComponentInChildren<Detect>();
        if (detect != null)
        {
            detect.OnEnter += OnDetectPlayerEnter;
            detect.OnStay += OnDetectPlayerStay;
            detect.OnExit += OnDetectPlayerExit;
        }
        else
        {
            Debug.LogError("Detect 컴포넌트를 찾을 수 없습니다.");
        }

        ////공격 범위 신호
        BossAttack1 boss1 = GetComponentInChildren<BossAttack1>();
        boss1.OnBoss1Enter += OnAttack1Enter;
        //boss1.OnBoss1Stay += OnAttck1Stay;
        boss1.OnBoss1Exit += OnAttack1Exit;

        BossAttack2 boss2 = GetComponentInChildren<BossAttack2>();
        boss2.OnBoss2Enter += OnAttack2Enter;
        boss2.OnBoss2Exit += OnAttack2Exit;
    }

    

    private void Start()
    {
        playerTrans = player.transform;
        player.OnDie += OnPlayerDied;

        StartCoroutine(transMovement());
    }

    private void Update()
    {
        
        if (isHit)
        {
            bossColor.material.color = new Color(255, 1, 1, 255);
            timer += Time.deltaTime;

        }
        
        if(timer == maxTimer)
        {
            bossColor.material.color = new Color(1, 1, 1, 255);
            isHit= false;
            timer= 0f;
        }
    }

    //FixedUpdate는 물리 시뮬레이션 갱신 주기에 맞춰서 호출된다. 
    private void FixedUpdate()
    {
        //몬스터움직임 함수
        MonsterMove();
    }


    //플레이어 감지 했을 때----------------------------------------------------------------------------------------------
    /// <summary>
    /// 감지됐다
    /// </summary>
    private void OnDetectPlayerEnter()
    {
        
        StopAllCoroutines();
        move = 1;
        find = true;
        
        anim.SetInteger("Move", move);
    }

    /// <summary>
    /// 감지 중.
    /// </summary>
    private void OnDetectPlayerStay()
    {
      
        monsterTransform = new Vector3(playerTrans.position.x, transform.position.y, playerTrans.position.z);
        transform.LookAt(monsterTransform);
        find = true;
        anim.SetInteger("Move", move);

    }

    /// <summary>
    /// 감지범위에서 나갔다.
    /// </summary>
    private void OnDetectPlayerExit()
    {
        find = false;

        //다시 자동 이동
        StartCoroutine(transMovement());
    }

    //플레이어와 보스에 거리마다 공격 패턴이 달라짐(범위가 긴 순서 1,2)---------------------------------------------------------------------------------------------
    private void OnAttack1Enter()
    {

        StopAllCoroutines();
        AttackMotion = 1;
        move = 0;
        anim.SetInteger("Attack", AttackMotion);


    }


    private void OnAttack1Exit()
    {
        AttackMotion = 0;
        move = 0;
        anim.SetInteger("Attack", AttackMotion);
        find = true;
    }
     
    private void OnAttack2Enter()
    {

        AttackMotion = 2;
        move = 0;
        anim.SetInteger("Attack", AttackMotion);
        StopAllCoroutines();
    }

    private void OnAttack2Exit()
    {
        AttackMotion = 1;
        move = 0;
        anim.SetInteger("Attack", AttackMotion);
        StopAllCoroutines();
    }


    //플레이어가 맵에 등장----------------------------------------------------------------------------
    //anim.SetBool("PlayerEnters" , true);


    /// <summary>
    /// 플레이어 죽은 신호 받고 실행
    /// </summary>
    void OnPlayerDied(int _)
    {
        find = false;
        anim.SetBool("PlayerDead", true);
        move = 0;
        //StartCoroutine(transMovement());
    }

    /// <summary>
    /// 자동 이동 시 몬스터 회전값 설정
    /// </summary>
    /// <returns></returns>
    IEnumerator transMovement()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {

            //move = 0 == Idle or move != 0 == Walk 실행
            move = UnityEngine.Random.Range(0, 2);
            anim.SetInteger("Move", move);

            if (move != 0)
            {
                transform.Rotate(0, transRotate, 0);  //좌우 회전
            }

            //5초 마다
            yield return new WaitForSeconds(5f);

        }

    }

    /// <summary>
    /// 몬스터 움직임(플레이어 찾았을 때/ 플레이어가 Scene에 없을 때/ 인식이 안되었을 때)
    /// </summary>
    private void MonsterMove()
    {

        //플레이어를 인식 했을 때
        if (find)
        {
            if (playerTrans != null)
            {

                StopAllCoroutines();
                //dir이 플레이어 방향 찾고 크기는 1 
                Vector3 dir = (playerTrans.position - transform.position).normalized;
                //몬스터 위치 + 속도 * DetaTime* 플레이 방향 
                rigid.MovePosition(transform.position + move * Time.fixedDeltaTime * dir);
            }
            else if (playerTrans == null)
            {
                move = 0;
                rigid.MovePosition(transform.position + Time.fixedDeltaTime * move * transform.forward);
                anim.SetBool("PlayerDead", true);
            }
        }
        //인식 안했을 때 행동

        else
            rigid.MovePosition(transform.position + move * Time.fixedDeltaTime  * transform.forward);


    }

    /// <summary>
    /// 이벤트 등록 해제
    /// </summary>
    void OnDestroy()
    {
        player.OnDie -= OnPlayerDied;
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
        anim.SetTrigger("Die");
        Destroy(gameObject,1.5f);


        Vector3 center = transform.position;
        center.y = 0.5f;

        GameObject obj = Instantiate(coinBox, center, Quaternion.identity);
        //죽었을 시 사망 애니메이션 실행 예정

    }
}
