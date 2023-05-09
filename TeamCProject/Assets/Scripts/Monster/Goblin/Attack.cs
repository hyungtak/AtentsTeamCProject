using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //데이미
    int damageAmount = 10;

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

    public void ColliderOff()
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
    }
    public void ColliderOn()
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = true;

    }



}
