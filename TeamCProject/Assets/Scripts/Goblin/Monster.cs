
using System.Collections;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class Monster : MonoBehaviour
{
    //몬스터 Hp int
    public int monsterMaxHp = 10;

    int currentMonsterHp = 10;

    //추가 할 것
    //몬스터 Attack  or Damage 설정
    //public int monsterDamage = 1;

    //public delegate void playerDied(bool find);

    Rigidbody rigid;
    Animator anim;

    /// <summary>
    /// 플레이어 위치 저장 할 변수
    /// </summary>
    Transform playerTrans;
    Player player;

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

        player= GetComponent<Player>();

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


        AttackMotion attack = FindObjectOfType<AttackMotion>();
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

    private void OnAttackEnter()
    {
        anim.SetBool("Attacko", true);
        find = true;

        //if (playerTrans == null)
        //{
        //    Debug.Log("플레이어는 null");
        //    MonsterMove();
        //    anim.SetBool("Attacko", false);

        //}
    }

    private void OnAttackExit()
    {
        anim.SetBool("Attacko", false);
        
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
        anim.SetBool("Jump", true);
        move = 0;
        //StartCoroutine(transMovement());
    }


//몬스터 감지 델리게이트--------------------------------------------------
    private void OnDetectPlayerEnter()
    {
        StopAllCoroutines();
        find = true;
        anim.SetBool("Run", true);
    }

    private void OnDetectPlayerStay()
    {
        monsterTransform = new Vector3(playerTrans.position.x, transform.position.y, playerTrans.position.z);
        transform.LookAt(monsterTransform);
        find = true;
        anim.SetBool("Run", true);
    }

    private void OnDetectPlayerExit()
    {
        //Run애니메이션 >> Walk로
        anim.SetBool("Run", false);
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
            //move = 0 == Idle or move != 0 == Walk 실행
            move = UnityEngine.Random.Range(0, 3);
            anim.SetInteger("Walk", move);

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

        //플레이어를 인식 했을 때
        
            
        if (find)
        {
            if (playerTrans != null)
            {
                StopAllCoroutines();
                move = 3;
                //dir이 플레이어 방향 찾고 크기는 1 
                Vector3 dir = (playerTrans.position - transform.position).normalized;
                //몬스터 위치 + 속도 * DetaTime* 플레이 방향 
                rigid.MovePosition(transform.position + move * Time.fixedDeltaTime * dir);
            }
            else if(playerTrans == null)
            {
                rigid.MovePosition(transform.position + Time.fixedDeltaTime * 0 * transform.forward);
                anim.SetBool("Jump", true);
            }
        }
        //인식 안했을 때 행동                              
        else
            rigid.MovePosition(transform.position + Time.fixedDeltaTime * move * transform.forward);


    }

    public void MonsterTakeDamage(int damageAmount)
    {
        currentMonsterHp -= damageAmount;

        if (currentMonsterHp <= 0)
        {
            MonsterDie();
        }
    }
    private void MonsterDie()
    {
        //죽었을 시 사망 애니메이션 실행 예정


        Destroy(gameObject);
    }
}
