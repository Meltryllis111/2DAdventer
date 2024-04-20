using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.CurrentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("IsRun", true);
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostCounter <= 0)
        {
            currentEnemy.Switchstate(NPCstate.Patrol);
        }
        if (!currentEnemy.pc.IsGround || (currentEnemy.pc.IsLeftWall && currentEnemy.faceDirection.x < 0) || (currentEnemy.pc.IsRightWall && currentEnemy.faceDirection.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDirection.x, 1, 1);
        }
    }
    public override void PhysicUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.lostCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("IsRun", false);
    }

}
