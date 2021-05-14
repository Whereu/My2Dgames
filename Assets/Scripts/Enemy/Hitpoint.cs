using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitpoint : MonoBehaviour
{
    public bool bombAvilable;

    int dir;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.position.x > other.transform.position.x)
            dir = -1;
        else
            dir = 1;

        if (other.CompareTag("Player"))
        {
            Debug.Log("������ˣ�");
            other.GetComponent<IDamageable>().GetHit(1);

        }
        if(other.CompareTag("Bomb")&& bombAvilable)
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, -1) * 10, ForceMode2D.Impulse);

        }
        
    }
}
