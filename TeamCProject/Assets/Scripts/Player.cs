using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Player : MonoBehaviour
{
    /// <summary>
    /// 임시 최대 체력
    /// </summary>
    public int maxHealth = 2;
    
    /// <summary>
    /// 임시 현재 체력
    /// </summary>
    public int currentHealth;

    /// <summary>
    /// 델리게이트 or 이벤트 선언 
    /// </summary>
    public delegate void PlayerDied();
    public static event PlayerDied playerDied;




    private void Start()
    {
        currentHealth = maxHealth;
        //Monster monster = FindObjectOfType<Monster>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }



    /// <summary>
    /// 죽었을 시 함수 
    /// </summary>
    private void Die()
    {
        if (playerDied != null)
        {
             playerDied();
        }
        Debug.Log("주금");
      
        gameObject.SetActive(false);
    }

}
