using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    Rigidbody2D rb2d;
    Animator animator;
    NewCharacter2DController controller;

    private string PLAYER_RUN = "Player_run1";
    private string PLAYER_IDLE = "Player_Idle1";
    private string PLAYER_JUMP = "Player_jump1";

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller = GetComponent<NewCharacter2DController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (controller.hasJumped)
        {
            animator.SetBool("hasJumped", true);
        }else
        {
            animator.SetBool("hasJumped", false);
        }

        if(controller.isFalling)
        {
            animator.SetBool("isFalling", true);
        } else
        {
            animator.SetBool("isFalling", false);
        }

        if(controller.isGrounded)
        {
            animator.SetBool("isGrounded", true);
        } else if(!controller.isGrounded)
        {
            animator.SetBool("isGrounded", false);
        }
        animator.SetFloat("vertical", controller.vertical);
    }

    
}
