using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount = 50;

    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void OnCollisionEnter(Collision colliosion)
    {
        if (colliosion.gameObject.CompareTag("Player"))
        {
          
            if (player != null)
            {
                player.TakeDamage(damageAmount);
                
            }
        }
    }
}
