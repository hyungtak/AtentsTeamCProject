
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{


    //확인용



    public float moveSpeed = 1f;

    //몬스터 타입이 0,1 ture false
    private int MonsterType;

    const int rotateSpeed = 90;

    //움직임 변환 변수
    private int move;
    private int sotpStart;
    private Transform player;

    //bool find = false;
    GameObject traceTarget;
    Animator anim;
    Rigidbody rigid;





    private void Awake()
    {

        //Component 가져오기
        rigid = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();
    }
    

    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //FristMonsterMove();
        StartCoroutine(transMovement());
;
    }

    //FixedUpdate는 물리 시뮬레이션 갱신 주기에 맞춰서 호출된다. 
    private void FixedUpdate()
    {

        
        MonsterMove();

        //if (find)
        //{
        //    //Vector3 player = traceTarget.transform.position;
        //    Vector3 dir = player.position - transform.position;
        //    dir.Normalize();
        //    rigid.velocity = dir * moveSpeed;

        //}
    }



    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        find = true;
    //       anim.SetInteger("Walk", 1);

    //    }
    //}

    IEnumerator transMovement()
    {
        

        while (true)
        {
            //범위의 랜덤을 설정 후 생성
            move = Random.Range(0, 2);
            
            anim.SetInteger("Walk", move);
            
            
            if(move == 1)
            {
                transform.localRotation = Quaternion.Euler(0, rotateSpeed, 0);
            }
            else if(move == 0) 
            {
                transform.localRotation = Quaternion.Euler(0, -rotateSpeed, 0);
            }

            yield return new WaitForSeconds(4f);
            //if(move == )

        }

    }

    //애니메이션 변화 
    //void transMovement()
    //{
    //    //범위의 랜덤을 설정 후 생성
    //    move = Random.Range(-1, 2); //-1, 0, 2 멈춤0 왼쪽-1 오른쪽1


    //    Invoke("transMovement", 3f);         //또 다시 호출해서 값을 받아온다

    //    anim.SetInteger("Walk", move);

    //    //    //if (move != 0)
    //    //    //{
    //    //    //    if (move == 1)
    //    //    //    {
    //    //    //        transform.Rotate(0, rotate, 0);
    //    //    //    }
    //    //    //}
    //}


    private void MonsterMove()
    {
        
        //rigid의 현재위치 + 초당 얼마 *속도_*
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * move * transform.forward);    //velocity.x,y,z는 0값이다.
                                                                                                            //move로 좌우측 계산
    }


    //private void FristMonsterMove()
    //{
    //    rigid.MovePosition(rigid.position + move * Time.fixedDeltaTime * moveSpeed * transform.forward);
    //}



}
