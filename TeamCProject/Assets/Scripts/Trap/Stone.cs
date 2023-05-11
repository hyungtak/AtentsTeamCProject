using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public float speed = 1.0f;

    private float rotateSpeed = 4.0f;

    public int damageAmount = 20;

    Rigidbody rb;

    private void OnEnable()
    {
        
        Destroy(gameObject, 2f);

        rb = GetComponent<Rigidbody>();

        // x, y, z축 모두 로컬 축을 기준으로 회전하도록 합니다.
        rb.angularVelocity = Random.insideUnitSphere * rotateSpeed;
    }


    public void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
    //    {
    //        yield return new WaitForSeconds(2f);
    //        Destroy(gameObject);
    //    }
}
