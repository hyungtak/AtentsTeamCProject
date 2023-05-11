using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRockDamage : MonoBehaviour
{

    public int damageAmount = 20;

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
