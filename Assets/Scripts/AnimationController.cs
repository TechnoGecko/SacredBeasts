using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    Rigidbody2D rb2d;
    Animator animator;
    NewCharacter2DController controller;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller = GetComponent<NewCharacter2DController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
