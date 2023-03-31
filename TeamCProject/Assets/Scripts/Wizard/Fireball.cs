using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Rigidbody rigid;
    private Vector3 fireBallTrnas;
    Transform playerTrans;

    private int damageAmount = 2;

    private void Awake()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Start()
    {

        FireTimer();
    }
    private void Update()
    {
        
        PlayerLookAt();
    }

    private void PlayerLookAt()
    {
        //fireBallTrnas = new Vector3(playerTrans.position.x, transform.position.y, playerTrans.position.z);
        //Vector3 dir = (playerTrans.position - transform.position).normalized;
        transform.position += Time.deltaTime * 10 * -transform.right;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Player playerHealth = other.GetComponent<Player>();
            if (playerHealth != null)
            {
                Debug.Log("충돌했다");
                playerHealth.TakeDamage(damageAmount);
            }

        }
    }


    IEnumerator FireTimer()
    {

        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }




}
