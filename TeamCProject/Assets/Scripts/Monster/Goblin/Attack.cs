using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //데이미
    public int damageAmount = 2;



    //공격 콜라이더가 플레이어에 적중 시
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            //Player 컴포넌트 가져옴
            Player playerHealth = other.GetComponent<Player>();
           
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

}
