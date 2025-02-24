using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float xForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool canJump;
    [SerializeField] private GameObject rainBow;
    [SerializeField] bool isClimbing = false;
    [SerializeField] float forceAmount = 10.0f;
    [SerializeField] float maxSpeed = 3.0f;
    
    private const string IS_WALKING_STR = "isWalking";
    private const string IS_JUMPING_STR = "canJump";
    private const float RAINBOW_OFFSET = 4.5f;
     
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private PlayerInput _playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        xForce = 1200.0f;
        jumpForce = 800.0f;
        
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        // Jumping
        Jump();
        // Player move or idle state
        MoveDirection();
        // Fire
        FireRainBow();
    }
        
    
    // Method Jump
    private void Jump()
    {
        
        _animator.SetFloat("jumpVelocity", _rb.velocity.y);
        if ( _playerInput.actions["Jump"].WasPressedThisFrame() /*Input.GetKeyDown(KeyCode.UpArrow)*/ && canJump)
        {
            _animator.SetBool(IS_WALKING_STR, false);
            _animator.SetBool(IS_JUMPING_STR, true);
            _rb.AddForce(new Vector2(0f, jumpForce));
            canJump = false;
        }
    }
    
    // Method Movedirections
    private void MoveDirection()
    {
        // Right, left, wait moves
        if(_playerInput.actions["DMove"].IsPressed() /*Input.GetKey(KeyCode.RightArrow)*/ && canJump)
        {
            _animator.SetBool(IS_WALKING_STR, true);
            _spriteRenderer.flipX = false;
            
            if (isClimbing && _rb.velocity.y >= 0.0f)
            {
                Climbing(1);
                return;
            }
            _rb.AddForce(new Vector2(xForce * Time.deltaTime, 0.0f));
            
        }else if (_playerInput.actions["LMove"].IsPressed()/*Input.GetKey(KeyCode.LeftArrow)*/ && canJump)
        {
            _animator.SetBool(IS_WALKING_STR, true);
            _spriteRenderer.flipX = true;
            
            if (isClimbing && _rb.velocity.y >= 0.0f)
            {
                Climbing(-1);
                return;
            }
            _rb.AddForce(new Vector2(-xForce * Time.deltaTime, 0.0f));
        }
        else
        {
            _animator.SetBool(IS_WALKING_STR, false);
        }
    }
    
    // Method FireRainBow
    private void FireRainBow()
    {
        if(_playerInput.actions["Fire"].WasPressedThisFrame()/*Input.GetKeyDown(KeyCode.Space)*/)
        {
            Instantiate(rainBow,
                new Vector3(transform.position.x +  (_spriteRenderer.flipX ? -RAINBOW_OFFSET : RAINBOW_OFFSET),
                    transform.position.y,
                    0.00f),
                transform.rotation);    
        }
    }
    
    // Method Climbing
    private void Climbing(float direction)
    {
        // Upper diagonal force "Left && Right direction"
        Vector2 diagonalForce = new Vector2(direction, 1).normalized * forceAmount;

        // Apply force to Rigidbody2D
        _rb.AddForce(diagonalForce);
                
        // Apply diagonal speed to rb
        _rb.velocity = diagonalForce;

        // Limit max velocity
        if (_rb.velocity.magnitude > maxSpeed) _rb.velocity = _rb.velocity.normalized * maxSpeed;
        
    }

    // Trigger OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        Vector2 normal = contact.normal;
        
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Rainbow"))
        {
            if (contact.normal.y > 0)
            {
               _animator.SetBool(IS_JUMPING_STR, false);
               canJump = true;
               isClimbing = false;
            }
        }
        
        if (collision.gameObject.CompareTag("Rainbow"))
        {
            
            if (normal.y >= 0.0f)
            {
                isClimbing = true;
                return;
            }

            isClimbing = false;
        }
    }
    
    // Trigger OnCollisionExit2D
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rainbow"))
        {
            isClimbing = false;
        }
    }
}
