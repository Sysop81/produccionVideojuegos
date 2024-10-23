using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float xForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool canJump;
    [SerializeField] private GameObject _rainBow;
    
    private const string IS_WALKING_STR = "isWalking";
    private const string IS_JUMPING_STR = "canJump";
    private const float RAINBOW_OFFSET = 4.5f;
    public bool _isClimbing = false;
    public float forceAmount = 30.0f;
    
    private Rigidbody2D rb;
    public float maxSpeed = 3.0f; 
    
    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        xForce = 1200.0f;
        jumpForce = 800.0f;
        
        rb = GetComponent<Rigidbody2D>();
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
            /*if (_isClimbing)
            {
                // Fuerza diagonal hacia arriba a la derecha
                Vector2 diagonalForce = new Vector2(1, 1).normalized * forceAmount;

                // Aplicar fuerza al Rigidbody2D del personaje
                gameObject.GetComponent<Rigidbody2D>().AddForce(diagonalForce);
                
                // Aplicar la velocidad diagonal directamente
                rb.velocity = diagonalForce;

                // Limitar la velocidad máxima
                if (rb.velocity.magnitude > maxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * maxSpeed;
                }
                //Debug.Log("andando en diagonal");
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce * Time.deltaTime, 0.0f));
                //Debug.Log("andando normal");
            }*/
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
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
            Instantiate(_rainBow,
                new Vector3(transform.position.x +  (gameObject.GetComponent<SpriteRenderer>().flipX ? -RAINBOW_OFFSET : RAINBOW_OFFSET),
                            transform.position.y,
                            0.00f),
                transform.rotation);    
        }
    }
        
    
    
    // Trigger OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Rainbow"))
        {
            gameObject.GetComponent<Animator>().SetBool(IS_JUMPING_STR, false);
            canJump = true;
            _isClimbing = false;
        }

        if (collision.gameObject.CompareTag("Rainbow"))
        {
            

            // Verify the normal collision
            ContactPoint2D contact = collision.contacts[0];
            Vector2 normal = contact.normal;
            Debug.Log(normal);
            if (normal.x != 0)
            {
                //Debug.Log("escalando");
                _isClimbing = true;
                return;
            }

            _isClimbing = false;

            //Debug.Log("collision with rainbow" + normal);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        Debug.Log(other.gameObject.name);
    }
}
