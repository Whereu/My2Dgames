using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
            enemy.aniState = 0;
            enemy.SwitchPoint();//两个点之间来巡移动
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            enemy.aniState = 1;
            enemy.MoveToTarget();
        }

        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f)
        {  
            enemy.TransitionToState(enemy.patrolState);//切换动画状态
        }

        if (enemy.attackList.Count > 0)//如果敌人在它们的视野范围内发现了目标，切换到攻击的状态

            enemy.TransitionToState(enemy.attackState);

    }
}
