using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    private Animator ani;
    private Collider2D coll;
    private Rigidbody2D rb;

    public float startTime, waitTime,bomoForce;

    [Header("Check")]
    public float radius;
    public LayerMask targetLayer;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;



    }

    // Update is called once per frame
    void Update()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Bomb_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                ani.Play("Bomb_explotion");

            }
        }
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);

    }
    /// <summary>
    /// 炸弹爆炸。并且产生各个方向的力对周围环境产生影响
    /// </summary>
    public void Explotion()//设置为动画事件
    {
        coll.enabled = false;
       
        Collider2D[] aroundobjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        rb.gravityScale = 0;
        foreach (var item in aroundobjects)
        {
            Vector3 pos = transform.position - item.transform.position;

            item.GetComponent<Rigidbody2D>().AddForce((-pos+Vector3.up) * bomoForce, ForceMode2D.Impulse);//（-pos+Vector3.up）反方向的力加上向上的力。

            if (item.CompareTag("Bomb") && item.GetComponent<Animator>().
                
                GetCurrentAnimatorStateInfo(0).IsName("Bomb_off"))
            {
                item.GetComponent<Bomb>().TrunOn();
            }
            if (item.CompareTag("Player"))

                item.GetComponent<IDamageable>().GetHit(3); 

        }


    }

    public void DestoryBomb()
    {

        Destroy(gameObject);

    }

    public void TrunOff()  
    {

        ani.Play("Bomb_off");
        gameObject.layer = LayerMask.NameToLayer("NPC");
    }
    public void TrunOn()
    {
        startTime = Time.time;
        ani.Play("Bomb_on");
        gameObject.layer = LayerMask.NameToLayer("Bomb");

    }
}
