using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;

    public int aniState;

    private GameObject alarmSign;
     
    [Header("Base State")]
    public float health;
    public bool isDead;


    public Animator anim;
    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    [Header("Attack Setting")]
    public float attackRate;
    private float nextAttack = 0;
    public float attackRange, skillRange;

    public List<Transform> attackList = new List<Transform>();

    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();


    // Start is called before the first frame update

    public virtual void Init()
    {

        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;
    }

    private void Awake()
    {
        Init();
    }


    void Start()
    {
        TransitionToState(patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
            return;
        currentState.OnUpdate(this);
        anim.SetInteger("state", aniState);
    }


  public  void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);

    }
    /// <summary>
    /// 移动到目标点
    /// </summary>
    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FlipDirection();

    }

    /// <summary>
    /// 攻击玩家
    /// </summary>
    public void AttackAction()
    {
       
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {   //播放攻击动画
                anim.SetTrigger("attack");
                Debug.Log("发现玩家，进行攻击!!!");
                nextAttack = Time.time + attackRate;
            }

        }


    }
    /// <summary>
    /// 攻击炸弹,不同的敌人技能不同
    /// </summary>
    public virtual void SkillAction()//virtual虚方法，子类可以对其重载修改。
    {
       

        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {   //播放技能攻击动画
                anim.SetTrigger("skill");
                Debug.Log("发现炸弹，使用技能攻击炸弹！！！");
                nextAttack = Time.time + attackRate;
            }

        }

    }
    /// <summary>
    /// 翻转敌人方向
    /// </summary>
    public void FlipDirection()
    {
        if (transform.position.x < targetPoint.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
         
    }
    /// <summary>
    /// 切换目标点
    /// </summary>
    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;

        }
        else
        {
            targetPoint = pointB;
        }


    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackList.Contains(collision.transform))//如果没有，再进行添加
        {
            attackList.Add(collision.transform);

        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(OnAlarm());

    }
    /// <summary>
    /// 执行显示警示标识的方法
    /// </summary>   
    /// alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length获取动画片段播放的时长。
    ///        
    /// 
    /// 
    /// 
    /// <returns></returns>
    IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>()
            .GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);



    }
}
