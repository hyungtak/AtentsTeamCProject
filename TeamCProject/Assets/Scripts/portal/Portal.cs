using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    /// <summary>
    /// 3대 맞으면 사망 카운트 형식으로 만들었다.
    /// </summary>
    public int PortalHp = 3;

    private int currentHp = 3;

    /// <summary>
    /// 무기에 맞았을 시 감소하는 숫자
    /// </summary>
    private const int hpMinus = 1;

    //주 공격 수단 몹소환(골렘 빼고)

    //몹 2마리 받기
    public GameObject goblin;
    public GameObject wizard;

    
    //무작정 소환되면 안되니까.
    //고블린 소환딜레이
    //위자드 소환딜레이
    WaitForSeconds goblinSummonTime = new WaitForSeconds(15);
    WaitForSeconds wizardSummonTime = new WaitForSeconds(30);
    /// <summary>
    /// 스킬 딜레이
    /// </summary>
    WaitForSeconds skillDelay = new WaitForSeconds(5);


    bool goblinActivate = false;
    bool wizardActivate = false;
    bool ColorChangeActivate = false;


    Transform monsterpos;
    Animator anim;
    StoneDown stoneDown;

    PortalColor portalColor;


    private void Awake()
    {
        currentHp = PortalHp;
        Transform child = transform.GetChild(0);
        monsterpos = child.GetComponent<Transform>();
        child = transform.GetChild(1);
        anim= child.GetComponent<Animator>();
        child = transform.GetChild(2);
        stoneDown = child.GetComponent<StoneDown>();
        child = transform.GetChild(3);
        portalColor = child.GetComponent<PortalColor>();

    }


    // 몬스터 소환, 보조스킬 사용
     private void Start()
    {

        StartCoroutine(goblinDelay());
        StartCoroutine(wizardDelay());
        StartCoroutine(StoneWave());

    }

    private void Update()
    {
        SummonGoblin();
        SummnonWizard();
        MaterialColor();
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

    /// <summary>
    /// 포탈 랜덤 스킬
    /// </summary>
    /// <returns></returns>
    IEnumerator StoneWave()
    {
        StopCoroutine(StoneRain());
        StopCoroutine(RockUp());
       
        int skill = UnityEngine.Random.Range(0, 3);
        yield return skillDelay;
        anim.SetBool("RockTF", false);

        if (skill == 1)
        {
            StartCoroutine(RockUp());
            Debug.Log("락");
                
        }

        else if (skill == 2)
        {
            StartCoroutine(StoneRain());
            Debug.Log("스톤");
        }

        else
        {
            Debug.Log("아무것도 하지않는다.");
            yield return skillDelay;
            StartCoroutine(StoneWave());
        }
    }

    IEnumerator RockUp()
    {
        StopCoroutine(StoneWave());
        StopCoroutine(StoneRain());

        anim.SetBool("RockTF",true);
        StartCoroutine(StoneWave());
        yield return null;
    }

    IEnumerator StoneRain()
    {
        StopCoroutine(StoneWave());
        StopCoroutine(RockUp());
        stoneDown.StoneStart();
        StartCoroutine(StoneWave());
        yield return null;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            currentHp -= hpMinus;
            ColorChangeActivate = true;

            if (currentHp <= 0)
            {
                breakPortal();
            }
        }
    }

    /// <summary>
    /// 포탈 데이미를 받았을 때 색상 변경
    /// </summary>
    private void MaterialColor()
    {
        if (ColorChangeActivate)
        {
            portalColor.ColorChange();
            ColorChangeActivate = false;
        }
    }


    /// <summary>
    /// 게임 끝
    /// </summary>
    void breakPortal()
    {
        Destroy(gameObject);
        StopAllCoroutines();
    }


}