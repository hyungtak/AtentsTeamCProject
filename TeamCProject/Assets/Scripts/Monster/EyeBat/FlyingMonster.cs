using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonster : MonoBehaviour
{
    public float speed = 5f; // 몬스터 이동 속도
    public float detectRange = 5f; // 플레이어 감지 범위
    public float chargeRange = 3f; // 돌진 범위
    public float chargeSpeed = 10f; // 돌진 속도
    public float returnTime = 3f; // 돌진 후 돌아오는 시간
    public float idleTime = 2f; // 제자리에서 대기하는 시간

    private Transform playerTransform; // 플레이어 Transform 컴포넌트
    private Vector3 initialPosition; // 몬스터의 초기 위치
    private bool isCharging; // 돌진 중인지 여부
    private bool isReturning; // 돌진 후 돌아오는 중인지 여부
    private float returnTimer; // 돌진 후 돌아오는 타이머
    private float idleTimer; // 제자리에서 대기하는 타이머

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;
        isCharging = false;
        isReturning = false;
        returnTimer = 0f;
        idleTimer = 0f;
    }

    private void Update()
    {
        if (!isCharging && !isReturning) // 돌진 중이 아닐 때
        {
            // 플레이어와의 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // 플레이어 감지 범위 내에 있으면 돌진 시작
            if (distanceToPlayer <= detectRange)
            {
                isCharging = true;
                transform.LookAt(playerTransform);
            }
        }
        else if (isCharging) // 돌진 중일 때
        {
            // 플레이어와의 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // 돌진 범위 내에 있으면 돌진 계속
            if (distanceToPlayer <= chargeRange)
            {
                transform.Translate(Vector3.forward * chargeSpeed * Time.deltaTime);
            }
            else // 돌진 범위 밖이면 돌아오기 시작
            {
                isCharging = false;
                isReturning = true;
                returnTimer = 0f;
            }
        }
        else if (isReturning) // 돌진 후 돌아오는 중일 때
        {
            returnTimer += Time.deltaTime;

            // 돌아온 후 일정 시간 대기
            if (returnTimer >= returnTime)
            {
                isReturning = false;
                idleTimer = 0f;
            }
            else
            {

            }

        }
    }
}