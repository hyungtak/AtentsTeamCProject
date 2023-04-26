using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    /// <summary>
    /// 웨이포인트 배열로 저장
    /// </summary>
    Transform[] waypoint;

    /// <summary>
    /// 웨이포인트 번호
    /// </summary>
    int index = 0;

    /// <summary>
    /// 현재 향하고 있는 웨이포인트의 트랜스폼 확인용 프로퍼티
    /// </summary>
    public Transform CurrentWaypoint => waypoint[index];

    private void Awake()
    {
        waypoint = new Transform[transform.childCount];
        for (int i = 0; i < waypoint.Length; i++)
        {
            waypoint[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// 다음에 이동해야 할 웨이포인트를 알려주는 함수
    /// </summary>
    /// <returns>다음에 이동할 웨이포인트의 트랜스폼</returns>
    public Transform GetNextWaypoint()
    {
        index++;                    // index 증가
        index %= waypoint.Length;  // index는 0~(waypoints.Length-1)까지만 되어야 한다.
        return waypoint[index];    // 해당 트랜스폼 리턴
    }
}
