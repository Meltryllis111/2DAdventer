using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnaillSkillState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.CurrentSpeed = 0;
        currentEnemy.anim.SetBool("IsWalk", false);
        currentEnemy.anim.SetBool("Hide", true);
        currentEnemy.anim.SetTrigger("Skill");

        currentEnemy.lostCounter = currentEnemy.lostTime;
        currentEnemy.GetComponent<Character>().IsInvincible = true;
        currentEnemy.GetComponent<Character>().InvincibleCounter = currentEnemy.lostCounter;
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostCounter <= 0)
        {
            currentEnemy.Switchstate(NPCstate.Patrol);
        }
        currentEnemy.GetComponent<Character>().InvincibleCounter = currentEnemy.lostCounter;
    }

    public override void PhysicUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Hide", false);
        currentEnemy.GetComponent<Character>().IsInvincible = false;
    }


}
