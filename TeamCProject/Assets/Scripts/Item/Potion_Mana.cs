using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // using 시켜서 Image 의 자료형으로 사용

public class Potion_Mana : MonoBehaviour
{
    Public string tpye;

    Potion_Mana = PM;

    /// <summary>
    /// 플레이어와 충돌 시 포션 즉시파괴
    /// </summary>
    void OnTriggerEnter(Collider PM)
    {
        if (PM.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
        /// <summary>
        /// 충돌한 것이 플레이어 일때 마나 50프로 회복
        /// </summary>
        else if (collision.CompareTag("Player") == true)
        {
            PlayerInfo.mp += PlayerInfo.mp * 0.5f;
            Debug.Log(마나업!);
        }
    }
