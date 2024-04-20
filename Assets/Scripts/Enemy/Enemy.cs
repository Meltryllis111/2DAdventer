using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicCheck))]
[RequireComponent(typeof(Character), typeof(Attack))]
public class Enemy : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PhysicCheck pc;

    [Header("基本属性")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector]public Vector3 spwanPoint;
    [HideInInspector] public float CurrentSpeed;
    public Vector3 faceDirection;
    public float hurtForce;
    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;
    [Header("计时器")]
    public float waitTime;
    [HideInInspector] public Transform attacker;
    public float waitCounter;
    public bool wait;
    public float lostTime;
    public float lostCounter;
    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    public bool canMove = true;

    protected BaseState currentState;
    protected BaseState patrolState;
    protected BaseState skillState;
    protected BaseState chaseState;

    #region 周期函数
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PhysicCheck>();

        CurrentSpeed = normalSpeed;
        waitCounter = waitTime;
        spwanPoint = transform.position;
    }
    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }
    private void Update()
    {
        faceDirection = new Vector3(-transform.localScale.x, 0, 0);
        currentState.LogicUpdate();
        TimeCounter();
    }
    private void FixedUpdate()
    {
        currentState.PhysicUpdate();
        if (!isHurt && !isDead && !wait && canMove)
        {
            Move();
        }


    }

    private void OnDisable()
    {
        currentState.OnExit();
    }
    #endregion


    public virtual void Move()
    {
        rb.velocity = new Vector2(CurrentSpeed * faceDirection.x * Time.deltaTime, rb.velocity.y);
    }

    public void TimeCounter()
    {
        if (wait)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter < 0)
            {
                wait = false;
                waitCounter = waitTime;
                transform.localScale = new Vector3(faceDirection.x, 1, 1);
            }
        }
        if (!FoundPlayer() && lostCounter > 0)
        {
            lostCounter -= Time.deltaTime;
        }
    }

    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDirection, checkDistance, attackLayer);
    }
    public void Switchstate(NPCstate state)
    {
        var newState = state switch
        {
            NPCstate.Patrol => patrolState,
            NPCstate.Chase => chaseState,
            NPCstate.Skill => skillState,
            _ => null,
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);
    }

    public virtual Vector3 GetNewPoint(){
        return transform.position;
    }

    #region 事件    
    public void onTakeDamage(Transform attackerTrans)
    {
        attacker = attackerTrans;
        if (attackerTrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (attackerTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //受伤击退
        isHurt = true;
        anim.SetTrigger("Hurt");
        Vector2 dir = new Vector2(transform.position.x - attackerTrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }

    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }

    public void OnDead()
    {
        gameObject.layer = 2;
        anim.SetBool("Dead", true);
        isDead = true;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
