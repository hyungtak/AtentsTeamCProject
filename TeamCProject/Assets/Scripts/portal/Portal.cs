using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.IO.Enumeration.FileSystemEnumerable<TResult>;

public class Portal : MonoBehaviour
{
    /// <summary>
    /// 3대 맞으면 사망
    /// </summary>
    public int PortalHp = 3;

    //주 공격 수단 몹소환(골렘 빼고)

    //몹 3마리 받기
    public GameObject goblin;
    public GameObject goblinPurple;
    public GameObject Wizard;

    //무작정 소환되면 안되니까.
    //고블린은 5초의 한번씩
    //위자드는 10초의 한번씩 소환
    WaitForSeconds monsterDelay = new WaitForSeconds(5);


    Transform monsterpos;
    //일정시간마다 순서대로 땅 또는 하늘에서 뭔가 떨어짐
    float delay = 5f;



    private void Awake()
    {
        monsterpos = transform.GetChild(0);
    }

}
