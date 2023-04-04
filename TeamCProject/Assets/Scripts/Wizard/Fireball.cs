using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody rigid;
    Transform playerTrans;
    private GameObject playerObj = null;

    private int damageAmount = 0;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {       
        StartCoroutine(FireFalseTimer());
        
    }

    private void Start()
    {
         playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

    }
    private void FixedUpdate()
    {
        
        PlayerLookAt();
    }

    private void PlayerLookAt()
    {
        Vector3 dir = (playerTrans.position - transform.position).normalized;
        dir.y = 0;
        rigid.MovePosition(transform.position + 5 * Time.fixedDeltaTime * dir);

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


    IEnumerator FireFalseTimer()
    {
        //Debug.Log("삭제");
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
    }




}
