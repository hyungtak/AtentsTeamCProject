using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    //몬스터 Hp int
    public int monsterMaxHp = 10;

    int currentMonsterHp = 10;

    public int monsterDamage = 1;


    /// <summary>
    /// 몬스터가 기본 회전값
    /// </summary>
    const int transRotate = 180;

    /// <summary>
    /// 플레이어를 감지했을 때 이동속도
    /// </summary>
    int moveSpeed = 2;

    /// <summary>
    /// 애니메이션 속도값
    /// </summary>
    private int move;



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

    protected virtual void Awake()
    {
        
        //필요한 Component 가져오기
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();

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

        //공격 범위 신호
        AttackMotion attack = GetComponentInChildren<AttackMotion>();
        if (attack != null)
        {
            attack.OnAttackEnter += OnAttackEnter;
            attack.OnAttackExit += OnAttackExit;
        }
        else
        {
            Debug.LogError("AttackMotion 컴포넌트를 찾을 수 없습니다.");
        }


    }

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        Player.playerDied += OnPlayerDied;

        StartCoroutine(transMovement());
    }


    //FixedUpdate는 물리 시뮬레이션 갱신 주기에 맞춰서 호출된다. 
    private void FixedUpdate()
    {
        //몬스터움직임 함수
        MonsterMove();
    }

    /// <summary>
    /// 이벤트 등록 해제
    /// </summary>
    void OnDestroy()
    {
        Player.playerDied -= OnPlayerDied;
    }

    //플레이어 감지 했을 때----------------------------------------------------------------------------------------------
    /// <summary>
    /// 감지됐다
    /// </summary>
    private void OnDetectPlayerEnter()
    {
        Debug.Log("감지버그확인");
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
        //Run애니메이션 >> Walk로
        find = false;

        //다시 자동 이동
        StartCoroutine(transMovement());
    }

    //플레이어가 공격 범위 내에 들어왔을 시 실행-----------------------------------------------------------------------
    private void OnAttackEnter()
    {
       
        //int randomAttack = 0;
        anim.SetBool("attackrr", true);
        move = 0;
        find = true;
        Debug.Log("공격버그확인");
    }


    private void OnAttackExit()
    {
        int randomAttack = -1;
        move = 1;
        anim.SetInteger("Attack", randomAttack);
        find = false;
        StartCoroutine(transMovement());
    }





    //플레이어가 맵에 등장----------------------------------------------------------------------------
    //anim.SetBool("PlayerEnters" , true);


    /// <summary>
    /// 플레이어 죽은 신호 받고 실행
    /// </summary>
    void OnPlayerDied()
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
        //죽었을 시 사망 애니메이션 실행 예정


        Destroy(gameObject);
    }
}
