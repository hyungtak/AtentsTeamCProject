
using System.Collections;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class MonsterPurple : MonoBehaviour
{
    //몬스터 Hp int
    public int monsterMaxHp = 10;

    int currentMonsterHp = 10;

    //추가 할 것
    //몬스터 Attack  or Damage 설정
    //public int monsterDamage = 1;

    //public delegate void playerDied(bool find);

    Rigidbody rigid;
    Animator anim;

    /// <summary>
    /// 플레이어 위치 저장 할 변수
    /// </summary>
    Transform playerTrans;
    Player player;

    /// <summary>
    /// 몬스터가 기본 회전값
    /// </summary>
    const int transRotate = 180;

    /// <summary>
    /// 애니메이션 변환값or속도값
    /// </summary>
    private int move;

    /// <summary>
    /// 플레이어 트리거 인식 false 인식x true 인식o
    /// </summary>
    //bool find = false;

    private Vector3 monsterTransform;

    private void Awake()
    {
        //필요한 Component 가져오기
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        player= GetComponent<Player>();

        Detect detect = GetComponentInChildren<Detect>();
        if (detect != null)
        {
            detect.OnEnter += OnDetectPlayerEnter;
            detect.OnExit += OnDetectPlayerExit;
        }
        else
        {
            Debug.LogError("Detect 컴포넌트를 찾을 수 없습니다.");
        }


    }


    

    private void Start()
    {
        playerTrans = player.transform;

        player.OnDie += OnPlayerDied;     
    }

    /// <summary>
    /// 이벤트 등록 해제
    /// </summary>
    void OnDestroy()
    {
        player.OnDie -= OnPlayerDied;
    }

    /// <summary>
    /// 신호 받고 몬스터 실행
    /// </summary>
    void OnPlayerDied(int _)
    {
        anim.SetTrigger("Dead");
    }


//몬스터 감지 델리게이트--------------------------------------------------
    private void OnDetectPlayerEnter()
    {
        anim.SetBool("Jump", true);
    }

    private void OnDetectPlayerExit()
    {
        //Run애니메이션 >> Walk로
        anim.SetBool("Jump", false);
    }

 

    public void MonsterTakeDamage(int damageAmount)
    {
        currentMonsterHp -= damageAmount;

        if (currentMonsterHp <= 0)
        {
            MonsterDie();
        }
    }
    private void MonsterDie()
    {

        anim.SetTrigger("Dead");
        //죽었을 시 사망 애니메이션 실행 예정
        Destroy(gameObject,1.5f);
    }
}
