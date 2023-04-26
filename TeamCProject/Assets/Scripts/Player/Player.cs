using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("플레이어 인스펙터------------------------------")]

    /// <summary>
    /// 이동속도
    /// </summary>
    public float moveSpeed = 2.0f;

    /// <summary>
    /// 점프력
    /// </summary>
    public float jumpForce = 5.0f;

    /// <summary>
    /// 입력처리용 InputAction
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 리지드 바디 컴포넌트
    /// </summary>
    Rigidbody rigid;

    /// <summary>
    /// 플레이어 애니메이션 컴포넌트
    /// </summary>
    Animator anim;

    /// <summary>
    /// 윗키 아래키 입력
    /// -1(아래) ~ 1(위) 사이
    /// </summary>
    //float upDownDirection = 0;

    /// <summary>
    /// 현재 이동
    /// -1(좌) ~ 1(우) 사이
    /// </summary>
    float moveDirection = 0;

    /// <summary>
    /// 최대 점프 가능 횟수
    /// 땅에 닿을 시 이 값으로 점프 가능 횟수 리셋
    /// </summary>
    int maxJumpCount = 2;

    /// <summary>
    /// 점프 가능 횟수
    /// 점프 1번당 1씩 소모
    /// </summary>
    int jumpCount = 2;

    /// <summary>
    /// 2단점프 할 때 벨로시티 수정을 위해 저장해둘 변수
    /// </summary>
    Vector3 jumpVelocity = Vector3.zero;

    /// <summary>
    /// 공격 딜레이용 변수
    /// </summary>
    WaitForSeconds attackDelayTime = new WaitForSeconds(1);

    /// <summary>
    /// 공격중에 공격이 안나가기 위한 bool 변수
    /// 참이면 공격중
    /// 거짓이면 공격 가능
    /// </summary>
    bool isAttack = false;

    //전준호씨 Player스크립트 변수
    //=====================================================================
    /// <summary>
    /// 임시 최대 체력
    /// </summary>
    public int maxHealth = 200;

    /// <summary>
    /// 임시 현재 체력
    /// </summary>
    public int currentHealth;

    int coinPoint = 0;

    public Action<int> CoinPlus;

    /// <summary>
    /// 델리게이트 or 이벤트 선언 
    /// </summary>
    public delegate void PlayerDied();
    public static event PlayerDied playerDied;


    public Action<int> onDie;

    //=====================================================================

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Attack.performed += onAttackInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= onAttackInput;
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))                   //Ground 와 충돌했을 때만
        {
            jumpCount = maxJumpCount;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();

        ///우리 플레이어는 좌, 우로만 움직이기에
        /// dir.x만 확인하면된다.

        moveDirection = dir.x;
        if (moveDirection == 1)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (moveDirection == -1)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            moveDirection *= -1;
        }
        anim.SetBool("IsMove", !context.canceled);
    }

    private void OnJumpInput(InputAction.CallbackContext obj)
    {
        if (jumpCount != 0)
        {
            //2단 점프를 위한 계산
            jumpVelocity = rigid.velocity;
            jumpVelocity.y = 0;
            rigid.velocity = jumpVelocity;

            rigid.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            jumpCount--;
        }
    }

    private void onAttackInput(InputAction.CallbackContext obj)
    {
        if(!isAttack)
        {
            isAttack = true;
            anim.SetTrigger("Attack");      
            StartCoroutine(DelayAttack());
        }
    }

    IEnumerator DelayAttack()
    {
        yield return attackDelayTime;
        isAttack = false;
    }

    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDirection * transform.forward);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"체력 : {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 죽었을 시 함수 
    /// </summary>
    private void Die()
    {
        if (playerDied != null)
        {
            onDie?.Invoke(coinPoint);
            playerDied();
        }  
    }

    /// <summary>
    /// 플레이어가 체력회복
    /// </summary>
    /// <param name="healing">포션 회복량</param>
    public void AddHP(int healing)
    {
        currentHealth += healing;
        Debug.Log($"HP: {currentHealth}");
    }

    public void AddCoin(int Point)
    {
        coinPoint += Point;
        
        Debug.Log($"CoinPoint추가 :{coinPoint}");

    }

}