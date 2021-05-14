using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;

    public float Speed;
    public float Jumpingforce;

    [Header("Player State")]
    public float health;
    public bool isDead;

    [Header("Ground Check")]
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("State Check")]
    public bool isGround;
    public bool canJump;
    public bool isJump;

    [Header("Jump FX")]
    public GameObject landFX, jumpFX;


    [Header("Attack Settings")]
    public GameObject bombPre;
    public float nextAttack = 0;
    public float attackRate;//炸弹爆炸频率


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
            return;
        
        CheckInput();
       

    }
    /// <summary>
    /// 物理运动方法建议放在FixUpdate中执行
    /// </summary>
    private void FixedUpdate()
    {
        if (isDead)//玩家角色死亡，设置速度为0，防止死亡后还会移动。
        {
            rb.velocity = Vector2.zero;
            return;
        }
        PhysicsCheck();
        Movement();
        Jump();
    }

    /// <summary>
    /// 人物移动
    /// </summary>
    private void Movement()
    {

       // float h = Input.GetAxis("Horizontal"); 
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxis("Vertical"); 

        rb.velocity = new Vector2(h* Speed, rb.velocity.y);
        if (h!=0)
        {
            transform.localScale = new Vector3(h, 1, 1);
        }

    }
    /// <summary>
    /// 监听按下了跳跃按键
    /// </summary>
    private void CheckInput()
    {
        if (Input.GetButtonDown("Jump")&&isGround)
        {
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();  
        }


    }

    /// <summary>
    /// 人物跳跃
    /// </summary>
    private void Jump()
    {
        if (canJump)
        {
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position + new Vector3(0,-0.45f,0);
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, Jumpingforce);
            rb.gravityScale = 4;
            canJump = false;
        }


    }
    /// <summary>
    /// 炸弹爆炸
    /// </summary>
    public void Attack()
    {
        if (Time.time>nextAttack)
        {
            Instantiate(bombPre, transform.position, bombPre.transform.rotation);
            nextAttack = Time.time + attackRate;

        }

        

    }

    private void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, groundLayer);
        if (isGround)
        {

            rb.gravityScale = 1;
            isJump = false;
        }

    }
    /// <summary>
    /// 落地动画 
    /// </summary>
    public void LandFX()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0,-0.75f,0);

    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundcheck.position,checkRadius);
    }

    public void GetHit(float damage)
    {
      
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Player_Hit"))
        {
            health -= damage;
            if (health < 1)
            {
                health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");

        }


    }
}
