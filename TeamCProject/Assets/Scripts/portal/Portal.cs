using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    /// <summary>
    /// 3대 맞으면 사망
    /// </summary>
    public int PortalHp = 3;

    //주 공격 수단 몹소환(골렘 빼고)

    //몹 3마리 받기
    public GameObject goblin;
    //public GameObject goblinPurple;
    public GameObject wizard;

    
    //무작정 소환되면 안되니까.
    //고블린은 5초의 한번씩
    //위자드는 10초의 한번씩 소환
    WaitForSeconds goblinSummonTime = new WaitForSeconds(5);
    WaitForSeconds wizardSummonTime = new WaitForSeconds(15);

    bool goblinActivate = false;
    bool wizardActivate = false;

    Transform monsterpos;
    //일정시간마다 순서대로 땅 또는 하늘에서 뭔가 떨어짐
    //float delay = 5f;



    private void Awake()
    {
        monsterpos = transform.GetChild(0);
    }

    private void Start()
    {

        StartCoroutine(goblinDelay());
        StartCoroutine(wizardDelay());
    }

    private void Update()
    {
        SummonGoblin();
        SummnonWizard();
    }





















    /// <summary>
    /// 고블린 소환
    /// </summary>
    private void SummonGoblin()
    {
        if (goblinActivate)
        {

            GameObject obj1 = Instantiate(goblin);
            obj1.transform.position = monsterpos.position;
            goblinActivate = false;
            StartCoroutine(goblinDelay());
        }
    }

    IEnumerator goblinDelay()
    {

        yield return goblinSummonTime;
        goblinActivate= true;
    }

    /// <summary>
    /// 스켈레톤위자드 소환
    /// </summary>
    private void SummnonWizard()
    {
        if (wizardActivate)
        {
            GameObject obj2 = Instantiate(wizard);
            obj2.transform.position = monsterpos.position;
            wizardActivate = false;
            StartCoroutine(wizardDelay());
        }
    } 

    IEnumerator wizardDelay()
    {
        yield return wizardSummonTime;
        wizardActivate = true;
    }

}
