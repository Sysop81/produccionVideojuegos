using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsHmove = Animator.StringToHash("isHmove");
    private static readonly int IsVmove = Animator.StringToHash("isVmove");
    
    [SerializeField] private float speedForce;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private GameObject[] lives;
    [SerializeField] GameManager gameManager;
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private float _hMove;
    private float _vMove;
    private Vector2 _movement;
    private bool _hasPowerUp;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        gameManager.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameState == GameState.GameOver) Destroy(gameObject);
        
        if (gameManager.gameState == GameState.InGame)
        {
            MoveDirection();
        }
        
    }
    
    private void MoveDirection() // TODO REFACT THIS METHOD!!
    {
        // Get move axes
        _hMove = Input.GetAxis("Horizontal");
        _vMove = Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //_rb.velocity = new Vector2(_hMove, 0).normalized * speedForce;
            _movement = new Vector2(_hMove, 0).normalized * speedForce;
            if (CheckMovement(Vector2.left))
            {
                _sr.flipX = true;
                _animator.SetBool(IsHmove, true);
                _animator.SetBool(IsVmove, false);
                _rb.velocity = _movement;
            }
            
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            /*_rb.velocity =*/ _movement = new Vector2(_hMove, 0).normalized * speedForce;
            if (CheckMovement(Vector2.right))
            {
                _sr.flipX = false;
                _animator.SetBool(IsHmove, true);
                _animator.SetBool(IsVmove, false);
                _rb.velocity = _movement;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            /*_rb.velocity*/ _movement = new Vector2(0,_vMove).normalized * speedForce;
            if (CheckMovement(Vector2.up))
            {
                _animator.SetBool(IsVmove, true);
                _animator.SetBool(IsHmove, false);
                _sr.flipY = true;
                _rb.velocity = _movement;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            /*_rb.velocity*/ _movement = new Vector2(0,_vMove).normalized * speedForce;
            if (CheckMovement(Vector2.down))
            {
                _animator.SetBool(IsVmove, true);
                _animator.SetBool(IsHmove, false);
                _sr.flipY = false;
                _rb.velocity = _movement;
            }
        }

    }

    private bool CheckMovement(Vector2 pDirection)
    {
        // Check direction with raycast 
        /*RaycastHit2D hit = Physics2D.Raycast(transform.position, pDirection, 0.5f, wallMask);
        Debug.DrawRay(transform.position, pDirection * 0.5f, Color.red);
        Debug.Log("RayCast hit -> " + hit.collider);
        //return true;
        return /*pDirection != Vector2.zero &&*/ //!hit.collider;*/
        
        
        Vector2 posicion1 = (Vector2)transform.position + Vector2.Perpendicular(pDirection) * 0.25f;
        Vector2 posicion2 = (Vector2)transform.position - Vector2.Perpendicular(pDirection) * 0.25f;

        
        RaycastHit2D hit1 = Physics2D.Raycast(posicion1, pDirection, 0.5f, wallMask);
        RaycastHit2D hit2 = Physics2D.Raycast(posicion2, pDirection, 0.5f, wallMask);
        
        
        Debug.DrawRay(posicion1, pDirection * 0.5f, Color.red);
        Debug.DrawRay(posicion2, pDirection * 0.5f, Color.red);
        
        return !hit1.collider && !hit2.collider;
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MagicDoor"))
        {
            Vector3 position;
            if (other.name.Contains("L"))
            {
                Debug.Log("left door");
                position = new Vector3(3.7f, other.transform.position.y, transform.position.z);
            }
            else
            {
                Debug.Log("right door");
                position = new Vector3(-3.7f, other.transform.position.y, transform.position.z);
            }
            
            transform.position = position;
        }

        if (other.CompareTag("Coin") || other.CompareTag("BigCoin"))
        {
            gameManager.UpdateScore(100);
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.CompareTag("Ghost"))
        {
            if (!_hasPowerUp)
            {
                gameManager.GameOver();
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            Debug.Log("collisio with Ghost");
        }
    }
}
