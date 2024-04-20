using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Vector3 target;
    private Attack attack;
    private Vector3 moveDir;
    private bool Isattack;
    private float attackRateCounter = 0;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.CurrentSpeed = currentEnemy.chaseSpeed;
        target = enemy.GetNewPoint();
        attack = enemy.GetComponent<Attack>();

        currentEnemy.lostCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("Chase", true);
    }
    public override void LogicUpdate()
    {
        //切换状态
        if (currentEnemy.lostCounter <= 0)
            currentEnemy.Switchstate(NPCstate.Patrol);
        //检测玩家坐标
        target = new Vector3(currentEnemy.attacker.position.x + 0.2f, currentEnemy.attacker.position.y + 1.6f, 0);
        //检测攻击范围
        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) <= attack.AttackRange && Mathf.Abs(currentEnemy.transform.position.y - target.y) <= attack.AttackRange)
        {
            //攻击
            Isattack = true;
            if (!currentEnemy.isHurt)
            {
                currentEnemy.rb.velocity = Vector2.zero;
            }

            //计时器
            attackRateCounter -= Time.deltaTime;
            if (attackRateCounter <= 0)
            {
                attackRateCounter = attack.AttackRate;
                currentEnemy.anim.SetTrigger("Attack");
            }
        }
        else
        {
            Isattack = false;
        }

        moveDir = (target - currentEnemy.transform.position).normalized;

        if (moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        else
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);

    }

    public override void PhysicUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDead && currentEnemy.canMove && !Isattack)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.CurrentSpeed * Time.deltaTime;
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Chase", false);
    }
}
