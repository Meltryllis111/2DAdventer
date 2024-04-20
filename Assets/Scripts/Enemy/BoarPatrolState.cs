using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.CurrentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.Switchstate(NPCstate.Chase);
        }

        if (!currentEnemy.pc.IsGround || (currentEnemy.pc.IsLeftWall && currentEnemy.faceDirection.x < 0) || (currentEnemy.pc.IsRightWall && currentEnemy.faceDirection.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("IsWalk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("IsWalk", true);
        }
    }

    public override void PhysicUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
