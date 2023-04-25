using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public PoolObjectType objectType;

    public float mix_X_Axis = -5f;
    public float max_X_Axis =  5f;

    public float interval = 2f;


    private Player player = null;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);  // 인터벌만큼 대기

            // 생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Factory.Inst.GetObject(objectType);

            // 생성한 게임오브젝트에서 컴포넌트 가져오기
            Stone stone = obj.GetComponent<Stone>();
            stone.TargetPlayer = player;
            stone.transform.position = transform.position;  // 스포너 위치로 이동

            // 상속 받은 클래스별 별도 처리
            OnSpawn(stone);
        }
    }

    private void OnSpawn(Stone stone)
    {
        float r = Random.Range(mix_X_Axis, max_X_Axis);             // 랜덤하게 적용할 기준 높이 구하고
        stone.transform.Translate(Vector3.up * r);      // 랜덤하게 높이 적용하기
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;             // 게임 전체적으로 색상 지정됨
                                                // Gizmos.color = new Color(0, 1, 0);   // rgb값으로 색상을 만들 수도 있다.

        // 스폰 영역을 큐브로 그리기
        Gizmos.DrawWireCube(transform.position,
            new Vector3(Mathf.Abs(max_X_Axis) + Mathf.Abs(mix_X_Axis) + 2, 1 , 1));
    }


}
