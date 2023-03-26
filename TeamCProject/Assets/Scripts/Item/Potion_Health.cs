using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // using 시켜서 Image 의 자료형으로 사용

public class Potion_Health : MonoBehaviour
{
    Public string tpye;

    Potion_Health = PH;

    /// <summary>
    /// 플레이어와 충돌 시 포션 즉시파괴
    /// </summary>
void OnTriggerEnter(Collider PH)
{
    if (PH.gameObject.name == "Player")
    {
        Destroy(gameObject);
    }
        /// <summary>
        /// 충돌한 것이 플레이어 일때 체력 30프로 회복
        /// </summary>
    else if (collision.CompareTag("Player") == true)
    {
        PlayerInfo.mp += PlayerInfo.mp * 0.3f;
        Debug.Log(체력업!);
    }

}
}
