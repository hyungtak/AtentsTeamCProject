
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{


    //확인용

    //몬스터 hp
    //몬스터 attack

    public float speedUp = 3f;

    //몬스터 타입이 0,1 ture false
    //private int MonsterType;

    const int transRotate = 180;

    //움직임 변환 변수
    private int move;
    

    private Transform player; //플레이어

    bool find = false;

    GameObject targetPlayer; 
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

    }

    //FixedUpdate는 물리 시뮬레이션 갱신 주기에 맞춰서 호출된다. 
    private void FixedUpdate()
    {
        MonsterMove();
        
    }

    //태그 될 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopCoroutine(transMovement());

            targetPlayer = other.gameObject;

            anim.SetBool("Run", true);
        }
        
    }

    //
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            find = true;
            StopCoroutine(transMovement());
        }
    }

    private void OnTriggerExit(Collider other)
    {
           find = false;

        if (other.gameObject.tag == "Player")
        {
           anim.SetBool("Run", false);
           StartCoroutine(transMovement());
        
        }
        
    }

    //몬스터 회전값 설정
    IEnumerator transMovement()
    {   

        //시간을 받는다.
         //float seconds = Random.Range(5f, 8f); 
    
            //범위의 랜덤을 설정 후 생성
         move = Random.Range(0, 3);           
         anim.SetInteger("Walk", move);
 
         if(move != 0)
         {
             transform.Rotate (0, transRotate, 0);  //좌우 회전
         }
          
         yield return new WaitForSeconds(5f);


        StartCoroutine(transMovement());        
        

    }


    //몬스터 움직임
    private void MonsterMove()
    {

        if(find)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            rigid.MovePosition(transform.position + dir * Time.fixedDeltaTime);
          

        }

        //rigid의 현재위치 + 초당 얼마 *속도*                          //move가 문제//move로 좌우측 계산
     
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime *transform.forward);    //velocity.x,y,z는 0값이다.
        
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(30, 10, 2));
    }


}
