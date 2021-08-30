using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController2 : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;




    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
         


    }

    private void FixedUpdate()
    {
        if(Input.GetKey("d") || Input.GetKey("right"))
        {
            rb2d.velocity = new Vector2(12, rb2d.velocity.y);
            animator.Play("Run");
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2d.velocity = new Vector2(-12, rb2d.velocity.y);
            animator.Play("Run");
            spriteRenderer.flipX = true;

        } else
        {
            animator.Play("Idle");
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if(Input.GetKey("space"))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 15);
            animator.Play("Jump");
        }
    }
}
