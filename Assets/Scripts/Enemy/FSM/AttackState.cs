using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        //Debug.Log("有敌人！！！！");
        enemy.aniState = 2;
        enemy.targetPoint = enemy.attackList[0];//切换敌人的攻击目标为列表中出现的第一个。

    }

    public override void OnUpdate(Enemy enemy)
    {
        if (enemy.attackList.Count == 0)
            enemy.TransitionToState(enemy.patrolState);

        if (enemy.attackList.Count > 1)
        {
            for (int i = 0; i < enemy.attackList.Count; i++)
            {
                if (Mathf.Abs(enemy.transform.position.x - enemy.attackList[i].position.x) <
                    Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))

                {
                    enemy.targetPoint = enemy.attackList[i];

                }
            }
        }
        if (enemy.attackList.Count == 1)
        {
            enemy.targetPoint = enemy.attackList[0];
        }

        if (enemy.targetPoint.CompareTag("Player"))//敌人根据Tag标签来区分当前遇到的是什么，并采取不同的攻击
            enemy.AttackAction();
        if (enemy.targetPoint.CompareTag("Bomb"))
            enemy.SkillAction();
        enemy.MoveToTarget();
    }
  
}
