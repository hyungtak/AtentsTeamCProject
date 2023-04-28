using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinPoint = 20;

    /// <summary>
    /// 아이템의 움직임 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 아이템이 움직이는 범위
    /// </summary>
    public float moveRange = 0.05f;

    /// <summary>
    /// 아이템의 시작지점 설정
    /// </summary>
    Vector3 startPoint;

    Player player;
    private void Awake()
    {
        player = FindObjectOfType<Player>();

        startPoint = transform.position;
          
    }

    private void Update()
    {
        float CoinY = Mathf.Sin(Time.time * moveSpeed) * moveRange;   //Sin을 사용하여 위아래 아이템의 위아래 움직임
        transform.position = startPoint + new Vector3(0, CoinY, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            //player.CoinCount += coinPoint;
        }
    }


}
