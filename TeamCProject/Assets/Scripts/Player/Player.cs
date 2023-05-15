using System;
using System.Collections;
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

    /// <summary>
    /// 최대 체력
    /// </summary>
    public int maxHealth = 200;

    /// <summary>
    /// 현재 체력
    /// </summary>
    public int currentHealth;

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
    /// 플레이어 사망시 쓸 델리게이트
    /// </summary>
    public Action<int> OnDie;

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
        CurrentHealth = maxHealth;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Attack.performed += OnAttackInput;
        inputActions.Player.Potion.performed += OnPotionInput;
        inputActions.Player.Exit.performed += OnExitInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Exit.performed -= OnExitInput;
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
            if(CurrentHealth < maxHealth)
            PotionCount--;
            CurrentHealth += 40;
        }
    }

    private void OnExitInput(InputAction.CallbackContext _)
    {
        ///임시방편
        ///추후, esc입력 시 메뉴창을 키고, 그 메뉴창에서 설정이나 종료를 다루도록 할 예정
        Application.Quit();
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
        OnDie?.Invoke(CoinCount);
    }

    
}
