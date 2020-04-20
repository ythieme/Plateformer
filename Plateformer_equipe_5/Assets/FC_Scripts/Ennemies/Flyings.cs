using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyings : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /*public void Chase()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        Vector2 lookDir = (Vector2)player.transform.position - rb.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        rb.rotation = angle;
        if (transform.position != Vector3.zero)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }*/
}
