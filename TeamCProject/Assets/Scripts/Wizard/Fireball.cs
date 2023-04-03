using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody rigid;
    private Vector3 fireBallTrnas;
    Transform playerTrans;

    private int damageAmount = 0;
    private void Awake()
    {

    }

    private void OnEnable()
    {
        
        //StartCoroutine(FireFalseTimer());
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        
        PlayerLookAt();
    }

    private void PlayerLookAt()
    {
        transform.position += Time.deltaTime * 10 * transform.forward;
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


    //IEnumerator FireFalseTimer()
    //{
    //    //Debug.Log("삭제");
    //    yield return new WaitForSeconds(10f);
    //    this.gameObject.SetActive(false);
    //}




}
