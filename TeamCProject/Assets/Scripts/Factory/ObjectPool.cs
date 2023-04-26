using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : UseObjectPool
{
    /// <summary>
    /// 풀에 생성해 놓을 오브젝트의 프리팹
    /// </summary>
    public GameObject originalPrefab;

    /// <summary>
    /// 풀의 크기. 처음에 생성하는 오브젝트의 갯수. 갯수는 2^n으로 잡는것이 좋다.
    /// </summary>
    public int poolSize = 16;

    /// <summary>
    /// 풀이 생성한 모든 오브젝트가 들어있는 배열
    /// </summary>
    T[] pool;

    /// <summary>
    /// 사용 가능한 오브젝트들이 들어있는 큐
    /// </summary>
    Queue<T> readyQueue;

    /// <summary>
    /// 처음 만들어졌을 때 한번 실행될 코드(초기화 코드)
    /// </summary>
    public void Initialize()
    {
        if (pool == null)
        {
            // 풀이 없으면 새로 만들고
            pool = new T[poolSize];
            readyQueue = new Queue<T>(poolSize);    // capacity를 poolSize만큼잡고 생성
            //readyQueue.Count();     // 실제로 사용하는 갯수
            //readyQueue.Capatity;    // 미리 만들어 놓은 노드의 갯수

            GenerateObjects(0, poolSize, pool);     // 첫번째 풀 생성
        }
        else
        {
            // 있으면 풀안의 오브젝트를 모두 비활성화 시킨다.
            foreach (var obj in pool)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 오브젝트를 생성하고 배열에 추가 하는 함수
    /// </summary>
    /// <param name="start">새로 생성한 오브젝트가 들어가기 시작할 인덱스</param>
    /// <param name="end">새로 생성한 오브젝트가 마지막으로 들어가는 인덱스의 한칸 앞</param>
    /// <param name="newArray">새로 생성한 오브젝트가 들어갈 배열(풀)</param>
    void GenerateObjects(int start, int end, T[] newArray)
    {
        for (int i = start; i < end; i++)    // start부터 end까지 반복
        {
            GameObject obj = Instantiate(originalPrefab, transform);    // 프리팹 생성하고 풀의 자식으로 설정
            obj.gameObject.name = $"{originalPrefab.name}_{i}";         // 이름 변경
            T comp = obj.GetComponent<T>();     // 컴포넌트 찾고(PoolObject 타입)

            // 리턴타입이 void이고 파라메터가 없는 람다함수를 onDisable에 등록
            // 델리게이트가 실행되면 readyQueue.Enqueue(comp) 실행
            comp.onDeactivate += () => readyQueue.Enqueue(comp);

            newArray[i] = comp;                 // 풀 배열에 넣고
            obj.SetActive(false);               // 비활성화해서 안보이게 만들기고 레디큐에도 추가하기
        }
    }

    /// <summary>
    /// 풀에서 오브젝트 하나를 꺼내서 리턴해주는 함수
    /// </summary>
    /// <returns>레디큐에서 꺼낸 오브젝트</returns>
    public T GetObject()
    {
        if (readyQueue.Count > 0)  // 큐에 오브젝트가 있는지 확인
        {
            T obj = readyQueue.Dequeue();   // 큐에 오브젝트가 있으면 큐에서 하나 꺼내고
            obj.gameObject.SetActive(true); // 활성화 시킨 다음에
            return obj;                     // 리턴
        }
        else
        {
            ExpandPool();           // 큐에 오브젝트가 없으면 풀을 두배로 늘린다.
            return GetObject();     // 새롭게 하나 요청
        }
    }

    public T GetObject(Transform spawnTransform)
    {
        if (readyQueue.Count > 0)  // 큐에 오브젝트가 있는지 확인
        {
            T obj = readyQueue.Dequeue();   // 큐에 오브젝트가 있으면 큐에서 하나 꺼내고
            obj.transform.position = spawnTransform.position;
            obj.transform.rotation = spawnTransform.rotation;
            obj.transform.localScale = spawnTransform.localScale;
            obj.gameObject.SetActive(true); // 활성화 시킨 다음에
            return obj;                     // 리턴
        }
        else
        {
            ExpandPool();           // 큐에 오브젝트가 없으면 풀을 두배로 늘린다.
            return GetObject();     // 새롭게 하나 요청
        }
    }

    /// <summary>
    /// 풀을 두배로 확장 시키는 함수
    /// </summary>
    private void ExpandPool()
    {
        Debug.LogWarning($"{this.gameObject.name} 풀 사이즈 증가");
        // 큐에 오브젝트가 없으면 풀을 두배로 늘린다.
        int newSize = poolSize * 2;     // 새 크기 설정
        T[] newPool = new T[newSize];   // 새 풀 생성
        for (int i = 0; i < poolSize; i++)// 이전 풀에 있던 내용을 새 풀에 복사
        {
            newPool[i] = pool[i];
        }
        GenerateObjects(poolSize, newSize, newPool);    // 이전 풀 이후 부분에 오브젝트 생성하고 새 풀에 추가
        pool = newPool;         // 새 풀을 풀로 설정
        poolSize = newSize;     // 사이즈로 새 풀 크기로 설정
    }
}
