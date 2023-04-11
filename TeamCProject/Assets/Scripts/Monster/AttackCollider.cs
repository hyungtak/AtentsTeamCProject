using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount = 2;

    private void OnCollisionEnter(Collision colliosion)
    {
      
        if (colliosion.gameObject.CompareTag("Player"))
        {
            Debug.Log("player와 충돌");
            //Player 컴포넌트 가져옴
            Player playerHealth = gameObject.GetComponent<Player>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("데미지 조건문 실행");
            }
        }
    }
}
