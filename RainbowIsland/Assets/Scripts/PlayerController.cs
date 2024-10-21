using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float xForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool canJump;
    
    private const string IS_WALKING_STR = "isWalking";
    private const string IS_JUMPING_STR = "canJump";
    
    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        xForce = 1200.0f;
        jumpForce = 800.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Jumping
        if (Input.GetKey("up") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool(IS_WALKING_STR, false);
            gameObject.GetComponent<Animator>().SetBool(IS_JUMPING_STR, true);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            canJump = false;
        }
        
        
        // Right, left, wait moves
        if(Input.GetKey("right") /*&& canJump*/)
        {
            gameObject.GetComponent<Animator>().SetBool(IS_WALKING_STR, true);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce * Time.deltaTime, 0.0f));
        }else if (Input.GetKey("left") /*&& canJump*/)
        {
            gameObject.GetComponent<Animator>().SetBool(IS_WALKING_STR, true);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-xForce * Time.deltaTime, 0.0f));
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool(IS_WALKING_STR, false);
        }
        
    }
    
    // Trigger OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.GetComponent<Animator>().SetBool(IS_JUMPING_STR, false);
            canJump = true;
        }
    }
}
