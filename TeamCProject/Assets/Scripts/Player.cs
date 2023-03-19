using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 2;
    public int currentHealth;


    private void Start()
    {
        currentHealth = maxHealth;

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Debug.Log("주금");
            gameObject.SetActive(false);
            Die();
        }
    }


    void Die()
    {
       
    }

}
