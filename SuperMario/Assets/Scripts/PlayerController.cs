using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private bool canJump;

    [SerializeField] private float xForce;
    [SerializeField] private float jumpForce;
    
    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        xForce = 400.0f;
        jumpForce = 400.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Jumping
        if (Input.GetKey("up") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", false);
            gameObject.GetComponent<Animator>().SetBool("isSaltando", true);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            canJump = false;
        }
        
        // Right, left, wait moves
        if(Input.GetKey("right") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce * Time.deltaTime, 0.0f));
        }else if (Input.GetKey("left") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-xForce * Time.deltaTime, 0.0f));
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", false);
        }        
        
    }
    
    // Trigger OnCollisionEnter2D
        void OnCollisionEnter2D(Collision2D collision)
        {
            // If player collision with ground change var canJump and reset animation
            if (collision.gameObject.tag == "ground")
            {
                gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
                canJump = true;
            }
        }
}
