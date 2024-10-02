using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyuController : MonoBehaviour
{
    [SerializeField]
    private bool canJump;

    //private float temporizador; Other form to set the collider with ground
    
    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        //temporizador = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Jumping
        if (Input.GetKey("up") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", false);
            gameObject.GetComponent<Animator>().SetBool("isSaltando", true);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 700f));
            canJump = false;
            //temporizador = 0f;
        }
        
        // Right, left, wait moves
        if(Input.GetKey("right") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(800.0f * Time.deltaTime, 0.0f));
        }else if (Input.GetKey("left") && canJump)
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-800.0f * Time.deltaTime, 0.0f));
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isAndando", false);
        }

        /*if (!canJump)
        {
            temporizador += Time.deltaTime;
        }

        if (!canJump && temporizador > 1.3f)
        {
            gameObject.GetComponent<Animator>().SetBool("isSaltando", false);
            canJump = true;
        }
        
        Debug.Log(gameObject.transform.position.y);*/
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
