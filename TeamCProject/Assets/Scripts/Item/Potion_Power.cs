using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Power : MonoBehaviour
{
    Public string tpye;

    Potion_Power = PP;

/// <summary>
/// 플레이어와 충돌 시 포션 즉시파괴
/// </summary>
void OnTriggerEnter(Collider PP)
    {
        if (PP.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// 충돌한 것이 플레이어일 경우 
        /// </summary>
        else if (collision.CompareTag("Player") == true)
        {
            // 공격력 대폭증가
        }
    }