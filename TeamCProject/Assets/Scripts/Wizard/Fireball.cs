using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Rigidbody rigid;
    Transform playerTrans;


    private int damageAmount = 0;
    private float speed = 5f;

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

        Vector3 playerPos = playerTrans.position;
        transform.rotation = Quaternion.LookRotation(playerPos);


    }
    //유도 공격
    //private void FixedUpdate()
    //{

    //   PlayerLookAt();
    //}

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //유도
    //private void PlayerLookAt()
    //{
    //    Vector3 dir = (playerTrans.position - transform.position).normalized;
    //    //dir.y = 0;
    //    rigid.MovePosition(transform.position + 5 * Time.fixedDeltaTime* d);

    //}


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
