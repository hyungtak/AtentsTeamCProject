using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    /// <summary>
    /// coin 오브젝트 받기
    /// </summary>
    public GameObject coin;

    /// <summary>
    /// 몬스터 최대 체력
    /// </summary>
    public int monsterMaxHp = 10;

    /// <summary>
    /// 몬스터 현재체력
    /// </summary>
    int currentMonsterHp = 10;

    /// <summary>
    /// 몬스터 공격 데미지
    /// </summary>
    public int monsterDamage = 1;

    /// <summary>
    /// 몬스터가 기본 회전값
    /// </summary>
    const int transRotate = 180;

    /// <summary>
    /// 애니메이션 변환값or속도값
    /// </summary>
    private int move;

    /// <summary>
    /// 플레이어 트리거 인식x = false 인식o = true 
    /// </summary>
    bool find = false;

    /// <summary>
    /// 몬스터가 플레이어에게 향하는 값 
    /// </summary>
    private Vector3 monsterTransform;

    Rigidbody rigid;

    /// <summary>
    /// 플레이어 위치 저장 할 변수
    /// </summary>
    Transform playerTrans;
    Player player;


    WaitForSeconds MoveStartDelay = new WaitForSeconds(1);
    WaitForSeconds MoveEndDelay = new WaitForSeconds(5);    

    protected virtual void Awake()
    {
        currentMonsterHp = monsterMaxHp;
        //필요한 Component 가져오기
        rigid = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Player>();


        //플레이어 감지 델리게이트
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
    }

    protected virtual void Start()
    {
        playerTrans = player.transform;
        player.OnDie += OnPlayerDied;

        StartCoroutine(transMovement());
    }


    //FixedUpdate는 물리 시뮬레이션 갱신 주기에 맞춰서 호출된다. 
    protected virtual void FixedUpdate()
    {
        //몬스터움직임 함수
        MonsterMove();
    }

    /// <summary>
    /// 이벤트 등록 해제
    /// </summary>
    void OnDestroy()
    {
        player.OnDie -= OnPlayerDied;
    }

    /// <summary>
    /// 플레이어 죽은 신호 받고 실행
    /// </summary>
    protected virtual void OnPlayerDied(int _)
    {
        find = false;
    }

    //플레이어 감지 했을 때----------------------------------------------------------------------------------------------
    /// <summary>
    /// 감지됐다
    /// </summary>
    protected virtual void OnDetectPlayerEnter()
    {
        StopAllCoroutines();
        
        move= 1;
        find = true;
    }



    /// <summary>
    /// 감지 중.
    /// </summary>
    protected virtual void OnDetectPlayerStay()
    {
        monsterTransform = new Vector3(playerTrans.position.x, transform.position.y, playerTrans.position.z);
        transform.LookAt(monsterTransform);
        find = true;
    }

    /// <summary>
    /// 감지범위에서 나갔다.
    /// </summary>
    protected virtual void OnDetectPlayerExit()
    {
        find = false;
        StartCoroutine(transMovement());
    }





    /// <summary>
    /// 자동 이동 시 몬스터 회전값 설정
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator transMovement()
    {

        yield return MoveStartDelay;
        while (true)
        {
            //move = 0 == Idle or move != 0 == Walk 실행
            move = Random.Range(0, 2);


            if (move != 0)
            {
                transform.Rotate(0, transRotate, 0);  //좌우 회전
            }

            //MoveEndDelay마다 반복 실행
            yield return MoveEndDelay;
        }
    }

    /// <summary>
    /// 몬스터 움직임(플레이어 찾았을 때/ 플레이어가 Scene에 없을 때/ 인식이 안되었을 때)
    /// </summary>
    protected virtual void MonsterMove()
    {
        //플레이어를 인식 했을 때
        if (find)
        {
            if (playerTrans != null)
            {
                StopAllCoroutines();

                Vector3 dir = (playerTrans.position - transform.position).normalized;

                rigid.MovePosition(transform.position + move * Time.fixedDeltaTime * dir);
            }
            else if (playerTrans == null)
            {
                rigid.MovePosition(transform.position + Time.fixedDeltaTime * 0 * transform.forward);
                //플레이어가 인식x 즉 죽었다? 그럴 때 사용되는 애니메이션 지정
            }
        }
        //인식 안했을 때 행동                              
        else
            rigid.MovePosition(transform.position + Time.fixedDeltaTime * moveSpeed * transform.forward);
    }


    /// <summary>
    /// 몬스터 데미지 받다
    /// </summary>
    /// <param name="damageAmount"></param>
    public virtual void MonsterTakeDamage(int damageAmount)
    {
        currentMonsterHp -= damageAmount;


        if (currentMonsterHp <= 0)
        {
            MonsterDie();
        }
    }

    /// <summary>
    /// 사망했을 때
    /// </summary>
    protected virtual void MonsterDie()
    { 


        Vector3 center = transform.position;
        center.y = 0.5f;
        GameObject obj = Instantiate(coin, center, Quaternion.identity);
    }
}

