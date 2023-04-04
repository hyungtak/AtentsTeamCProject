using DigitalRuby.PyroParticles;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public GameObject fireBall;

    

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

    Rigidbody rigid;
    Animator anim;

    Transform fireTransform;
    
    /// <summary>
    /// 플레이어 위치 저장 할 변수
    /// </summary>
    Transform playerTrans;

    Player player;
    private void Awake()
    {

        //필요한 Component 가져오기
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = gameObject.GetComponent<Player>();
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
        anim.SetBool("Dead", true);
        move= 0;
        //StartCoroutine();
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
        
        anim.SetBool("Attack", true);
        move = 1;
        monsterTransform = new Vector3(playerTrans.position.x, transform.position.y, playerTrans.position.z);
        transform.LookAt(monsterTransform);
        find = true;

    }

    private void OnDetectPlayerExit()
    {
        //Run애니메이션 >> Walk로
        move = 0;
        anim.SetBool("Attack", false);
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

            //5초 마다
            yield return new WaitForSeconds(5f);

        }

    }

    //IEnumerator FireInst()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    StartCoroutine(FireInst());
    //    Debug.Log("발사");
    //    GameObject obj = Instantiate(fireBall);
    //    obj.transform.position = fireTransform.position;
        
        


    //}


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
                //move = 3;
                //dir이 플레이어 방향 찾고 크기는 1 
                Vector3 dir = (playerTrans.position - transform.position).normalized;
                //몬스터 위치 + 속도 * DetaTime* 플레이 방향 
                rigid.MovePosition(transform.position + 0* Time.fixedDeltaTime * dir);
            }
            else if (playerTrans == null)
            {
                Debug.Log("플레이어null");
                rigid.MovePosition(transform.position + Time.fixedDeltaTime * 0 * transform.forward);
                anim.SetBool("Dead", true);
            }
        }

        //인식 안했을 때 행동                              
        else
            rigid.MovePosition(transform.position + Time.fixedDeltaTime * (move * 0.5f) * transform.forward);

    }


    public void FireStart()
    {
        Debug.Log("불불");
        GameObject obj = Instantiate(fireBall);
        obj.transform.position = fireTransform.position;
    }






}
