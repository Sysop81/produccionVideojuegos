using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class GhostController : MonoBehaviour
{
    
    private static readonly int IsHmove = Animator.StringToHash("IsHmove");
    private static readonly int IsUp = Animator.StringToHash("IsUp");
    private static readonly int IsDown = Animator.StringToHash("IsDown");
    private static readonly int IsScary = Animator.StringToHash("isScary");
    
    
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private Transform player;
    [SerializeField] GameManager gameManager;
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private int _hMove,_vMove;
    private float[] _movements;
    private PlayerController _playerController;
    private bool _isDead;
    private bool _isScary;
    private const float SPEED = 1.0f;
    private const float LOW_SPEED = 0.7f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _hMove = 0;
        _vMove = -1;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _playerController = player.gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(gameManager.gameState == GameState.GameOver) Destroy(gameObject);
        
        /*if (player)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, wallMask);
            //ManageAnimationDirection(direction);
            if (!hit.collider)
            {
                _rb.velocity = direction * 1.5f;
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }



            //MoveDirection((int)direction.x, (int)direction.y);
            //transform.position +=  (Vector3)direction * 2.0f * Time.deltaTime;
        }*/
        
        MoveDirection(_hMove,_vMove);

        if (_playerController.GetHasPowerUp() && !_isScary)
        {
            _animator.SetTrigger(IsScary);
            _isScary = false;
        }

        if (_isDead)
        {
            
        }
    }

    public void SetIsDead(bool isDead)
    {
        _isDead = isDead;
    }

    public void SetIsScary(bool isScary)
    {
        _isScary = isScary;
    }

    /*void ManageAnimationDirection(Vector2 direction)
    {

        if (direction.x > 0)
        {
            Debug.Log("Derecha " + direction);
        }else if (direction.x < 0)
        {
            Debug.Log("Izquierda " + direction);
        }else if (direction.y > 0)
        {
            Debug.Log("Subir " + direction);
        }
        else
        {
            Debug.Log("Bajar " + direction);
        }


        /*if (direction.x > 0)
        {
            Debug.Log("derecha");
            _animator.SetBool(IsHmove, true);
            _sr.flipX = direction == Vector2.right ;
            _animator.SetBool(IsUp, false);
            _animator.SetBool(IsDown, false);

        }else if (direction == Vector2.up)
        {
            Debug.Log("ENTRO SEGUNDO IF");
            _animator.SetBool(IsHmove, false);
            _animator.SetBool(IsUp, true);
            _animator.SetBool(IsDown, false);
        }
        else
        {
            Debug.Log("ENTRO ELSE");
            _animator.SetBool(IsHmove, false);
            _animator.SetBool(IsUp, false);
            _animator.SetBool(IsDown, true);
        }

    }*/

    void MoveDirection(int hMove, int vMove)
    {
        _rb.velocity = new Vector2(hMove,vMove).normalized * (_playerController.GetHasPowerUp() ? SPEED : LOW_SPEED);
    }
    
    /*private bool CheckMovement(Vector2 pDirection)
    {
        
        Vector2 posicion1 = (Vector2)transform.position + Vector2.Perpendicular(pDirection) * 0.25f;
        Vector2 posicion2 = (Vector2)transform.position - Vector2.Perpendicular(pDirection) * 0.25f;

        
        RaycastHit2D hit1 = Physics2D.Raycast(posicion1, pDirection, 0.5f, wallMask);
        RaycastHit2D hit2 = Physics2D.Raycast(posicion2, pDirection, 0.5f, wallMask);
        
        
        Debug.DrawRay(posicion1, pDirection * 0.5f, Color.red);
        Debug.DrawRay(posicion2, pDirection * 0.5f, Color.red);
        
        return !hit1.collider && !hit2.collider;
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {

            if (_vMove < 0)
            {
                
                _animator.SetBool(IsUp, true);
                _animator.SetBool(IsDown, false);
                _vMove = 1;
            }
            else
            {
                _animator.SetBool(IsUp, false);
                _animator.SetBool(IsDown, true);
                _vMove = -1;
            }
        }
    }

    private Vector2 GetRandomMovement()
    {
        Vector2[] movements = {Vector2.up, Vector2.left, Vector2.down, Vector2.right };
        
        return movements[Random.Range(0, movements.Length)];
    }
}
