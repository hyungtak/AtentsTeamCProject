using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : UseObjectPool
{
    /// <summary>
    /// 게임오브젝트 프리팹 
    /// </summary>
    public GameObject prefabs;

    /// <summary>
    /// 풀 갯수
    /// </summary>
    public int poolSize = 16;

    /// <summary>
    /// 오브젝트가 들어갈 배열
    /// </summary>
    T[] pool;

    Queue<T> readyQueue;



    public void Initialize()
    {
        if (pool == null)
        {
            // 풀이 없으면 새로 만들고
            pool = new T[poolSize];
            readyQueue = new Queue<T>(poolSize);

            GenerateObjects(0, poolSize, pool); // 풀 생성
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

    void GenerateObjects(int start, int end, T[] newArray)
    {
        for (int i = start; i < end; i++)
        {
            GameObject obj = Instantiate(prefabs, transform);   // 프리팹 생성하고 풀의 자식으로 설정
            obj.gameObject.name = $"{prefabs.name}({i})";       //생성되는 오브젝트 이름
            T component = obj.GetComponent<T>();     //컴포넌트 찾기

            component.deactivate += () => readyQueue.Enqueue(component);  //component 객체를 readyQueue 큐에 추가하여 재사용 가능한 상태로 만든다.

            newArray[i] = component;            // 풀 배열에 넣고
            obj.SetActive(false);               //비활성화
        }
    }



    public T GetObject()
    {
        if (readyQueue.Count > 0)  //오브젝트가 있다
        {
            T obj = readyQueue.Dequeue();   // 큐에 오브젝트가 있으면 큐에서 하나 꺼내고
            obj.gameObject.SetActive(true); // 활성화 시킨 다음에
            return obj;                     // 리턴
        }
        else                                //오브젝트에 없다.
        {
            TwicePool();                   //2배로 늘리기
            return GetObject();  
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
            TwicePool();          // 큐에 오브젝트가 없으면 풀을 두배로 늘린다.
            return GetObject();     // 새롭게 하나 요청
        }
    }

    private void TwicePool()
    {
        Debug.LogWarning($"{this.gameObject.name} 2배 증가");

        int newSize = poolSize * 2;     // 2배 늘리기
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
