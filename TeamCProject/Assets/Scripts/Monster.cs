using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //확인용


    public int monsterMove;
    //public int nextMove;
    int left = -1;
    
    

    Animator anim;
    Rigidbody rigid;
    
    
    

    private void Awake()    
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Invoke("backMove", 5);

    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector3(monsterMove, rigid.velocity.y, 0);
        //Vector3 movement = new Vector3(monsterMove, 0, 0);

        //rigid.MovePosition(rigid.position + monsterMove * Time.fixedDeltaTime);
        //Vector3 front = new Vector3(Time.deltaTime*rigid.position.x + monsterMove, rigid.position.y, rigid.position.z);





    }

    void backMove()
    {
        //범위의 랜덤을 설정 후 생성
        monsterMove = Random.Range(-1, 2); //-1, 0, 1 왼쪽 멈춤 오른쪽


        Invoke("backMove", 5);
        anim.SetInteger("walkSpeed", monsterMove);
        //if(monsterMove != 0)
        //{
        //    transform.Rotate(0, 0, 180);
        //}
    }

}