using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        pc.IsGround = true;
        patrolState = new SnailPatrolState();
        skillState = new SnaillSkillState();
    }
}
