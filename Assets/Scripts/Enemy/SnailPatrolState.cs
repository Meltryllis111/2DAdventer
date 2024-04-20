using UnityEngine.Tilemaps;

public class SnailPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.CurrentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
        //检测玩家
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.Switchstate(NPCstate.Skill);
        }
        //检测地面和墙壁
        if (!currentEnemy.pc.IsGround || (currentEnemy.pc.IsLeftWall && currentEnemy.faceDirection.x < 0) || (currentEnemy.pc.IsRightWall && currentEnemy.faceDirection.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("IsWalk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("IsWalk", true);
        }
        //检测动画状态
        if (currentEnemy.anim.GetCurrentAnimatorStateInfo(0).IsName("PreMove") || currentEnemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Recover"))
            currentEnemy.canMove = false;
        else
            currentEnemy.canMove = true;
       
        
    }
    public override void PhysicUpdate()
    {

    }


    public override void OnExit()
    {

    }

}
