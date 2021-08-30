using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fiend : MonoBehaviour
{
     Rigidbody2D rb;
    SpriteRenderer sr;
    

    public const string RIGHT = "right";
    public const string LEFT = "left";

    string buttonPressed;

    [SerializeField] float moveSpeed = 5;
     
     

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            buttonPressed = RIGHT;
            transform.Translate(Vector2.right * (Time.deltaTime * moveSpeed), Space.World);
            
        }
        else if (Input.GetKey(KeyCode.A))
        {
            buttonPressed = LEFT;
            transform.Translate(Vector2.left * (Time.deltaTime * moveSpeed), Space.World);
            
        }
        else
        {
            buttonPressed = null;
        }

    }

    private void FixedUpdate()
    {

        if (buttonPressed == RIGHT)
        {
            
        }
        else if (buttonPressed == LEFT)
        {
            

        } else
        {
            
        }
       
    }


}
