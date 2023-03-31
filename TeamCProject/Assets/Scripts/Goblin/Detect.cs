using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Detect : MonoBehaviour
{
   

    public Action OnEnter;
    public Action OnStay;
    public Action OnExit; 


    /// <summary>
    /// 플레이어가 트리거의 접촉
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            OnEnter?.Invoke();
        }
    }


    /// <summary>
    /// 플레이어가 트리거 안에 지속적으로 인식
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnStay?.Invoke();

        }
    }


    /// <summary>
    /// 플레이어가 트리거 밖으로 나갔을 때 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            OnExit?.Invoke();
        }

    }


    /// <summary>
    /// 공격감지 기즈모 
    /// </summary>
    private void OnDrawGizmos()
    {

        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.color = Color.red;

            Vector3 monsterSenter = transform.position;
            monsterSenter.y = 50f;

            // 로컬 좌표 값을 월드로 변환
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawWireCube(monsterSenter, boxCollider.size);
        }



    }


}
