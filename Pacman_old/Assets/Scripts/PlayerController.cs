using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsHmove = Animator.StringToHash("isHmove");
    private static readonly int IsVmove = Animator.StringToHash("isVmove");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    
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
    private Vector3 _initialTransform;
    private const int POWER_UP_TIME = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        _initialTransform = transform.position;
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
        Vector2 position1 = (Vector2)transform.position + Vector2.Perpendicular(pDirection) * 0.25f;
        Vector2 position2 = (Vector2)transform.position - Vector2.Perpendicular(pDirection) * 0.25f;
        Vector2 position3 = transform.position;

        
        RaycastHit2D hit1 = Physics2D.Raycast(position1, pDirection, 0.5f, wallMask);
        RaycastHit2D hit2 = Physics2D.Raycast(position2, pDirection, 0.5f, wallMask);
        RaycastHit2D hit3 = Physics2D.Raycast(position3, pDirection, 0.5f, wallMask);
        
        
        Debug.DrawRay(position1, pDirection * 0.5f, Color.red);
        Debug.DrawRay(position2, pDirection * 0.5f, Color.red);
        Debug.DrawRay(position3, pDirection * 0.5f, Color.green);
        
        return !hit1.collider && !hit2.collider && !hit3.collider;
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
            if (other.CompareTag("BigCoin")) StartCoroutine(ManagePowerUp());
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.CompareTag("Ghost"))
        {
            if (!_hasPowerUp)
            {
                gameManager.UpdateGhostVisibility(false);
                StartCoroutine(ManagePlayerDeath());
                return;
            }
            
            other.gameObject.GetComponent<GhostController>().SetIsDead(true);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            Debug.Log("collisio with Ghost");
        }
    }*/

    IEnumerator ManagePlayerDeath()
    {
        
        _animator.SetTrigger(IsDead);
        yield return new WaitForSeconds(2f);
        transform.position = _initialTransform;
        gameManager.GameOver();
        gameManager.UpdateGhostVisibility(true);
    }

    IEnumerator ManagePowerUp()
    {
        _hasPowerUp = true;
        yield return new WaitForSeconds(POWER_UP_TIME);
        _hasPowerUp = false;
    }

    public bool GetHasPowerUp()
    {
        return _hasPowerUp;
    }
}
