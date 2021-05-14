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
    /// �ƶ���Ŀ���
    /// </summary>
    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FlipDirection();

    }

    /// <summary>
    /// �������
    /// </summary>
    public void AttackAction()
    {
       
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {   //���Ź�������
                anim.SetTrigger("attack");
                Debug.Log("������ң����й���!!!");
                nextAttack = Time.time + attackRate;
            }

        }


    }
    /// <summary>
    /// ����ը��,��ͬ�ĵ��˼��ܲ�ͬ
    /// </summary>
    public virtual void SkillAction()//virtual�鷽����������Զ��������޸ġ�
    {
       

        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {   //���ż��ܹ�������
                anim.SetTrigger("skill");
                Debug.Log("����ը����ʹ�ü��ܹ���ը��������");
                nextAttack = Time.time + attackRate;
            }

        }

    }
    /// <summary>
    /// ��ת���˷���
    /// </summary>
    public void FlipDirection()
    {
        if (transform.position.x < targetPoint.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
         
    }
    /// <summary>
    /// �л�Ŀ���
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
        if (!attackList.Contains(collision.transform))//���û�У��ٽ������
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
    /// ִ����ʾ��ʾ��ʶ�ķ���
    /// </summary>   
    /// alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length��ȡ����Ƭ�β��ŵ�ʱ����
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
