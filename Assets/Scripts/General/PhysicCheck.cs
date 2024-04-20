using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    private Rigidbody2D rb;
    private PlayerController playerController;
    [Header("检测参数")]
    public bool IsPlayer;
    public bool manual;
    public Vector2 BottomOffset;
    public Vector2 LeftOffset;
    public Vector2 RightOffset;
    public float checkRadius;
    public LayerMask groundLayer;
    [Header("状态")]
    public bool IsGround;
    public bool IsLeftWall;
    public bool IsRightWall;
    public bool IsWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (!manual)
        {
            LeftOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            RightOffset = new Vector2(-(coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
        }
        if (IsPlayer)
        {
            playerController = GetComponent<PlayerController>();
        }
    }
    void Update()
    {
        Check();
    }

    public void Check()
    {
        //地面检测
        if (IsWall)
            IsGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(BottomOffset.x * transform.localScale.x, BottomOffset.y), checkRadius, groundLayer);
        else
            IsGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(BottomOffset.x * transform.localScale.x, 0), checkRadius, groundLayer);
        //墙体判断
        IsLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + LeftOffset, checkRadius, groundLayer);
        IsRightWall = Physics2D.OverlapCircle((Vector2)transform.position + RightOffset, checkRadius, groundLayer);
        //判断在墙上且不在地面上
        if (IsPlayer)
            IsWall = (IsLeftWall && playerController.inputDirection.x > 0f || IsRightWall && playerController.inputDirection.x < 0f) && rb.velocity.y < 0f;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(BottomOffset.x * transform.localScale.x, BottomOffset.y), checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + LeftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + RightOffset, checkRadius);
    }
}
