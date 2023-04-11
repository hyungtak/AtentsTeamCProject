using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount = 50;

    private void OnCollisionEnter(Collision colliosion)
    {
        if (colliosion.gameObject.CompareTag("Player"))
        {
          
            //Player 컴포넌트 가져옴
            Player playerHealth = FindObjectOfType<Player>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                
            }
        }
    }
}
