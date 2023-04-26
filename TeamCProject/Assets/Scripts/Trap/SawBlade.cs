using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    /// <summary>
    /// 회전속도 
    /// </summary>
    public float spinSpeed = 360.0f;

    /// <summary>
    /// 이동속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 다음 장소
    /// </summary>
    public WayPoint targetWaypoints;

    /// <summary>
    /// 이동 방향
    /// </summary>
    Vector3 moveDir;

    /// <summary>
    /// 목적지 웨이포인트
    /// </summary>
    Transform target;

    /// <summary>
    /// 칼날회전하기
    /// </summary>
    Transform sawBlade;

    /// <summary>
    /// 이번 프레임에 이동한 정도
    /// </summary>
    protected Vector3 moveDelta = Vector3.zero;


    private void Awake()
    {
        sawBlade = transform.GetChild(0);
    }

    private void Start()
    {
        SetTarget(targetWaypoints.CurrentWaypoint); // 첫번째 웨이포인트 지점 설정
    }

    private void FixedUpdate()
    {
        MovePosition();
    }

    private void Update()
    {
        sawBlade.Rotate(Time.deltaTime * spinSpeed * Vector3.forward);
    }


    private void MovePosition()
    {
        moveDelta = Time.fixedDeltaTime * moveSpeed * moveDir;              // 이동 방향으로 움직이기
        transform.Translate(moveDelta, Space.World);   // 이동

        if ((target.position - transform.position).sqrMagnitude < 0.01f)    // 거리가 0.1보다 작을 때
        {
            // 도착
            SetTarget(targetWaypoints.GetNextWaypoint());   // 도착했으면 다음 웨이포인트 지점 가져와서 설정하기
            moveDelta = Vector3.zero;
        }

    }


    private void SetTarget(Transform target)
    {
        this.target = target;               // 목적지 설정하고        
        moveDir = (this.target.position - transform.position).normalized; // 이동 방향 기록해 놓기
        transform.LookAt(target);
    }

}
