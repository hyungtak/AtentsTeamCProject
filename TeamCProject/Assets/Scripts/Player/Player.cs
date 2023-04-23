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
    /// 현재 입력된 입력 방향
    /// </summary>
    private Vector3 inputDir = Vector3.zero;

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
    /// 점프 중인지 확인용 변수
    /// 참이면 점프중, 거짓이면 점프 중 아님
    /// </summary>
    bool isJump = false;

    WaitForSeconds attackDelayTime = new WaitForSeconds(1);

    /// <summary>
    /// 공격중에 공격이 안나가기 위한 bool 변수
    /// 참이면 공격중
    /// 거짓이면 공격 가능
    /// </summary>
    bool isAttack = false;

    /// <summary>
    /// 최대 체력
    /// </summary>
    public int maxHealth = 200;

    /// <summary>
    /// 현재 체력
    /// </summary>
    public int currentHealth;

    int coinPoint = 0;

    public Action<int> CoinPlus;

    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHpChange?.Invoke(currentHealth);
        }
    }
    public Action<int> OnHpChange;

    /// <summary>
    /// 델리게이트 or 이벤트 선언 
    /// </summary>
    public delegate void PlayerDied();
    public static event PlayerDied playerDied;




    //=====================================================================
    /// <summary>
    /// 최대 포션 소지량
    /// </summary>
    public int maxPotionCount = 5;

    /// <summary>
    /// 포션 소지량
    /// </summary>
    int potionCount = int.MinValue;
    public int PotionCount
    {
        get => potionCount;

        set
        {
            potionCount = value;
            onPotionCountChange?.Invoke(potionCount);
        }
    }
    public Action<int> onPotionCountChange;

    /// <summary>
    /// 코인 소지량
    /// </summary>
    int coinCount = int.MinValue;
    public int CoinCount
    {
        get => coinCount;
        set
        {
            coinCount = value;
            onCoinCountChange?.Invoke(coinCount);
        }
    }
    public Action<int> onCoinCountChange;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
        anim = GetComponent<Animator>();

        coinCount = 0;
        potionCount = maxPotionCount;
    }

    private void Start()
    {
        //currentHealth = maxHealth;
        //Monster monster = FindObjectOfType<Monster>();
        Debug.Log($"처음 CoinPoint :{coinPoint}");
        CurrentHealth = maxHealth;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Attack.performed += OnAttackInput;
        inputActions.Player.Potion.performed += OnPotionInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Potion.performed -= OnPotionInput;
        inputActions.Player.Attack.performed -= OnAttackInput;
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))                   //Ground 와 충돌했을 때만
        {
            isJump = false;
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
            rigid.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (moveDirection == -1)
        {
            rigid.rotation = Quaternion.Euler(0, 270, 0);

            moveDirection *= -1;
        }
        anim.SetBool("IsMove", !context.canceled);
    }

    private void OnJumpInput(InputAction.CallbackContext _)
    {
        if (!isJump)
        {
            rigid.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isJump = true;
        }
    }

    private void OnAttackInput(InputAction.CallbackContext _)
    {
        if(!isAttack)
        {
            isAttack = true;
            anim.SetTrigger("Attack");      
            StartCoroutine(DelayAttack());
        }
    }

    private void OnPotionInput(InputAction.CallbackContext _)
    {
        if(PotionCount > 0)
        {
            PotionCount--;
            CurrentHealth += 40;
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
        CurrentHealth -= damageAmount;
        Debug.Log($"체력 : {CurrentHealth}");
        if (CurrentHealth <= 0)
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

        //UI작업 시 사용
        //CoinPlus?.Invoke(coinPoint);

    }

}
