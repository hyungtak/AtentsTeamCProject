using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    
    /// <summary>
    /// 파이어볼
    /// </summary>
    public GameObject fireBall;

    /// <summary>
    /// 코인
    /// </summary>
    public GameObject coin;

    
    public int monsterMaxHp = 10;
    
    int currentMonsterHp = 100;

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

    Transform fireTransform;

    
    Rigidbody rigid;
    Animator anim;
    /// <summary>
    /// 플레이어 위치 저장 할 변수
    /// </summary>
    Transform playerTrans;

    Player player;
    private void Awake()
    {
        currentMonsterHp = monsterMaxHp;

        //필요한 Component 가져오기
        rigid = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Player>();

        anim = GetComponent<Animator>();

        fireTransform = transform.GetChild(0);

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

    


    private void Start()
    {
        playerTrans = player.transform;

        player.OnDie += OnPlayerDied;

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
        player.OnDie -= OnPlayerDied;  
    }

    /// <summary>
    /// 신호 받고 몬스터 실행
    /// </summary>
    void OnPlayerDied(int _)
    {
        find = false;
        anim.SetBool("Dead", true);
        move= 0;
        //StartCoroutine();
    }


    private void OnDetectPlayerEnter()
    {
        StopAllCoroutines();
        find = true;
        StartCoroutine(fireballDelay());
    }

    private void OnDetectPlayerStay()
    {

        
        move = 1;

        monsterTransform = new Vector3(playerTrans.position.x, transform.position.y, playerTrans.position.z);
        transform.LookAt(monsterTransform);
        find = true;
        

    }

    private void OnDetectPlayerExit()
    { 
        move = 0;
        anim.SetBool("Attack", false);

        find = false;
        StartCoroutine(transMovement());

    }
    
    /// <summary>
    /// 자동 이동 시 몬스터 회전값 설정
    /// </summary>
    /// <returns></returns>
    IEnumerator transMovement()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            //move = 0이면 Idle or move != 0이면 Walk 실행
            move = Random.Range(0, 3);

            anim.SetInteger("forward", move);

            if (move != 0)
            {
                transform.Rotate(0, transRotate, 0);  //좌우 회전
            }

            //5초 마다
            yield return new WaitForSeconds(5f);

        }

    }

    IEnumerator fireballDelay()
    {
        //yield return new WaitForSeconds(1f);
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
       
    }



    /// <summary>
    /// 몬스터 움직임
    /// </summary>
    private void MonsterMove()
    {
        if (find)
        {
            move = 0;
            if (playerTrans != null)
            {
                StopAllCoroutines();

                Vector3 dir = (playerTrans.position - transform.position).normalized;

                rigid.MovePosition(transform.position + move * Time.fixedDeltaTime * dir);
            }

            else if (playerTrans == null)
            {
                rigid.MovePosition(transform.position + Time.fixedDeltaTime * move * transform.forward);

                anim.SetBool("Dead", true);
            }
        }

        //인식 안했을 때 행동                              
        else
            rigid.MovePosition(transform.position + Time.fixedDeltaTime * (move * 0.5f) * transform.forward);

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
        anim.SetBool("Damage", true);

        Invoke("Test",1f);
        

        if (currentMonsterHp <= 0)
        {
            MonsterDie();
        }
    }
    private void MonsterDie()
    {
        StopAllCoroutines();
        anim.SetTrigger("Dead");
        Destroy(gameObject, 1.5f);

        Vector3 center = transform.position;
        center.y = 1.5f;

        GameObject obj = Instantiate(coin, center, Quaternion.identity);
        //죽었을 시 사망 애니메이션 실행 예정
    }

    void Test()
    {
        anim.SetBool("Damage", false);
    }

}
