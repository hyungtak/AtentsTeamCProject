using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    protected bool Attack = false;

    /// <summary>
    /// 플레이어 위치 저장 할 변수
    /// </summary>
    protected Transform player;

   


    protected virtual void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
  
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            Attack = true;

        }
    }






}

