using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioDefination))]
public class PlayerController : MonoBehaviour
{
    [Header("获取组件")]
    public GameObject pasuePanel;
    [Header("监听事件")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterLoadEvent;
    public VoidEventSO loadDataEvent;
    public PlayerInputControl inputControl;
    private PhysicCheck physicCheck;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private SpriteRenderer sr;
    private Character character;
    private PlayerAnimation pa;
    public Vector2 inputDirection;
    public bool isCrouch;
    [Header("基本参数")]
    public float speed;
    public float JumpForce;
    public float WallJumpForce;
    private float runSpeed;
    private Vector2 originColliderOffset;
    private Vector2 originColliderSize;
    public int slidePowerCost;
    private float walkSpeed => runSpeed / 2.5f;
    public bool ishurt;
    public bool isDead = false;
    public float hurtForce;
    public bool isAttack;
    public bool isSlide;
    public float slideDistance;
    public float slideSpeed;
    [HideInInspector] public bool WallJump;
    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicCheck = GetComponent<PhysicCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        pa = GetComponent<PlayerAnimation>();
        originColliderOffset = coll.offset;
        originColliderSize = coll.size;
        character = GetComponent<Character>();
        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.Slide.started += Slide;
        inputControl.Gameplay.Settings.started += Settings;

        #region 强制走路
        runSpeed = speed;
        inputControl.Gameplay.WalkButton.performed += ctx =>
        {
            if (physicCheck.IsGround)
                speed = walkSpeed;
        };
        inputControl.Gameplay.WalkButton.canceled += ctx =>
        {
            if (physicCheck.IsGround)
                speed = runSpeed;
        };
        #endregion
        #region 攻击
        inputControl.Gameplay.AttackButton.started += playerAttack;
        #endregion
    }



    private void OnEnable()
    {
        inputControl.Enable();
        sceneLoadEvent.LoadRequestedEvnet += OnLoadEvent;
        afterLoadEvent.OnEventRaised += OnAfterLoadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
    }


    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestedEvnet -= OnLoadEvent;
        afterLoadEvent.OnEventRaised -= OnAfterLoadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
    }

    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if (!ishurt && !isDead && !isAttack)
            Move();
    }

    // private void OnTriggerStay2D(Collider2D other) {
    //     Debug.Log(other.name);
    // }
    private void OnAfterLoadEvent()
    {
        inputControl.Gameplay.Enable();
    }
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }
    public void Move()
    {
        if (isSlide)
            return;
        if (!isCrouch && !WallJump)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        //人物翻转
        if (inputDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (inputDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //蹲下
        isCrouch = inputDirection.y < -0.5f && physicCheck.IsGround;
        //修改碰撞体
        if (isCrouch)
        {
            //修改碰撞体
            coll.size = new Vector2(coll.size.x, 1.7f);
            coll.offset = new Vector2(coll.offset.x, 0.85f);
        }
        else
        {
            //还原碰撞体
            coll.size = originColliderSize;
            coll.offset = originColliderOffset;
        }

    }

    private void Slide(InputAction.CallbackContext context)
    {
        if (!isSlide && physicCheck.IsGround && character.currentPower >= slidePowerCost)
        {
            isSlide = true;

            var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            StartCoroutine(TriggerSlide(targetPos));

            character.OnSlide(slidePowerCost);
        }
    }
    private void Settings(InputAction.CallbackContext context)
    {
        if (pasuePanel.activeInHierarchy)
        {
            pasuePanel.SetActive(false);
        }
        else
        {
            pasuePanel.SetActive(true);
        }
    }

    private IEnumerator TriggerSlide(Vector3 targetPos)
    {
        do
        {
            yield return null;
            if (!physicCheck.IsGround)
            {
                break;
            }
            if (physicCheck.IsLeftWall && transform.localScale.x < 0f || physicCheck.IsRightWall && transform.localScale.x > 0f)
            {
                isSlide = false;
                break;
            }
            rb.MovePosition(new Vector2(transform.position.x + slideSpeed * transform.localScale.x, transform.position.y));
        } while (MathF.Abs(targetPos.x - transform.position.x) > 0.1f);
        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }


    private void Jump(InputAction.CallbackContext context)
    {
        // Debug.Log("Jump");
        if (physicCheck.IsGround)
        {
            rb.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
            GetComponent<AudioDefination>().PlayAudio();
            isSlide = false;
            StopAllCoroutines();
        }

        else if (physicCheck.IsWall)
        {
            rb.AddForce(new Vector2(-inputDirection.x, 2f) * WallJumpForce, ForceMode2D.Impulse);
            WallJump = true;
        }
    }
    private void playerAttack(InputAction.CallbackContext context)
    {
        pa.PlayAttack();
        isAttack = true;

    }
    public void GetHurt(Transform attacker)
    {
        ishurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }

    public void CheckState()
    {
        coll.sharedMaterial = physicCheck.IsGround ? normal : wall;
        if (physicCheck.IsWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2.0f);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if (WallJump && rb.velocity.y < 0f)
            WallJump = false;
    }

}
