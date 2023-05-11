using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class Potion : MonoBehaviour
{
    Transform healingPotion;

    public int healing = 50;

    /// <summary>
    /// 아이템의 움직임 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 아이템이 움직이는 범위
    /// </summary>
    public float moveRange = 0.5f;

    float RotateMove = 120f;

    /// <summary>
    /// 아이템의 시작지점 설정
    /// </summary>
    Vector3 startPoint;

    Player player;
    private void Awake()
    {
        healingPotion = transform.GetChild(0);
        startPoint = transform.position;

        player = FindObjectOfType<Player>();

    }

    private void Update()
    {
        float potionY = Mathf.Sin(Time.time * moveSpeed) * moveRange;   //Sin을 사용하여 위아래 아이템의 위아래 움직임
        transform.position = startPoint + new Vector3(0, potionY, 0);

        healingPotion.Rotate(Time.deltaTime * RotateMove * Vector3.forward);    //회전

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);       //사용

        }
    }
}