using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
            enemy.aniState = 0;
            enemy.SwitchPoint();//������֮����Ѳ�ƶ�
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
            enemy.TransitionToState(enemy.patrolState);//�л�����״̬
        }

        if (enemy.attackList.Count > 0)//������������ǵ���Ұ��Χ�ڷ�����Ŀ�꣬�л���������״̬

            enemy.TransitionToState(enemy.attackState);

    }
}
