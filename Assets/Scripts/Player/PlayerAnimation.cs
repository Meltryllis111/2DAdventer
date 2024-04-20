using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicCheck physicCheck;
    private PlayerController pc;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicCheck = GetComponent<PhysicCheck>();
        pc = GetComponent<PlayerController>();
    }
    
    private void Update() 
    {
        SetAnimation();
    }
    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("IsGround", physicCheck.IsGround);
        anim.SetBool("IsCrouch", pc.isCrouch);
        anim.SetBool("IsDead", pc.isDead);
        anim.SetBool("IsAttack", pc.isAttack);
        anim.SetBool("IsWall", physicCheck.IsWall);
        anim.SetBool("IsSlide", pc.isSlide);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("Hurt");
    }
    public void PlayAttack()
    {
        anim.SetTrigger("Attack");
    }
}
