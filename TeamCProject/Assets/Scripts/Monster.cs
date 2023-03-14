
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

    const int rotate = 180;

    //움직임 변환 변수
    private int move;

    private Transform player;

    bool find = false;
    GameObject traceTarget;
    Animator anim;
    Rigidbody rigid;





    private void Awake()
    {

        //Component 가져오기
        rigid = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();

        //Invoke로
        Invoke("transMovement", 3f);

    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Vector3 dir = player.position - transform.position;
        //dir.Normalize();
        //GetComponent<Rigidbody>().velocity = dir * moveSpeed;
    }

    //FixedUpdate는 물리 시뮬레이션 갱신 주기에 맞춰서 호출된다. 
    private void FixedUpdate()
    {


        MonsterMove();
        //findPalyer();

        //rigid.velocity = Time.deltaTime* moveSpeed * movement;
        //Vector3 movement = new Vector3(monsterMove, 0, 0);
        //rigid.MovePosition(rigid.position + monsterMove * Time.fixedDeltaTime);

    }



    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           
           anim.SetInteger("Walk", 1);
        
        }
    }


    //애니메이션 변화 
    void transMovement()
    {
        //범위의 랜덤을 설정 후 생성
        move = Random.Range(0, 3); //0, 1, 2 멈춤0 왼쪽1 오른쪽2


        Invoke("transMovement", 3f);         //또 다시 호출해서 값을 받아온다

        anim.SetInteger("Walk", move);

        if (move != 0)
        {
            if (move == 1)
            {
                transform.Rotate(0, rotate, 0);
            }
        }
    }
    //좀 더 생각


    //    //    //else(monsterMove == 0)
    //    //    //{
    //    //    //    transform.Rotate(0, rotate, rotate);
    //    //    //}
    //    //}
    //}


    //private void findPalyer()
    //{
    //    if (find)
    //    {
    //        Vector3 direction = player.position - transform.position;
    //        direction.Normalize();
    //        GetComponent<Rigidbody>().velocity = direction * moveSpeed;
    //    }
    //}

    private void MonsterMove()
    {

        if (find)
        {
            //Vector3 player = traceTarget.transform.position;
            Vector3 dir = player.position - transform.position;
                dir.Normalize();
                GetComponent<Rigidbody>().velocity = dir * moveSpeed;

        }

        rigid.velocity = new Vector3(move, rigid.velocity.y, rigid.velocity.z);    //velocity.x,y,z는 0값이다.
                                                                                   //move로 좌우측 계산


    }




}
