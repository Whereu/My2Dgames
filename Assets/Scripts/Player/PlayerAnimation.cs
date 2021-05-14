using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator ani;
    private Rigidbody2D rb;
    private PlayerController controller;
    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetFloat("speed", Mathf.Abs(rb.velocity.x));//�ٶȻ�С��0��ȡ�ٶȵľ���ֵ
        ani.SetFloat("velocityY", rb.velocity.y);

        ani.SetBool("jump", controller.isJump);
        ani.SetBool("ground", controller.isGround);



    }
}
